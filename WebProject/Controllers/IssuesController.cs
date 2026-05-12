using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebProject.Attributes;
using WebProject.DataAccess;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers;

[AuthorizePermission((int)Pages.Issues, (int)PageAccess.Read)]
public class IssuesController(WebProjectDbContext _context) : Controller
{
    // ================= INDEX =================
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var data = await _context.Issues
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => new IssueManagementVM
            {
                Id = x.Id,
                CreatedTime = x.CreatedTime,
                Category = x.Category,
                SubCategory = x.SubCategory,
                Status = x.Status,
            })
            .ToListAsync(ct);

        return View(data);
    }

    // ================= DETAILS =================
    public async Task<IActionResult> Details(int id, CancellationToken ct = default)
    {
        var data = await _context.Issues
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new IssueDetailsVM
            {
                Category = x.Category,
                SubCategory = x.SubCategory,
                Description = x.Note,
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime,
            })
            .FirstOrDefaultAsync(ct);

        if (data == null) return NotFound(ct);

        return View(data);
    }

    // ================= CREATE =================
    [AuthorizePermission((int)Pages.Issues, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Create(CancellationToken ct = default)
    {
        IssueCreateVM vm = new()
        {
            Categories = Enum.GetNames<Pages>(),
            SubCategories = Enum.GetNames<PageAccess>(),
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthorizePermission((int)Pages.Issues, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Create(IssueCreateVM vm, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = Enum.GetNames<Pages>();
            vm.SubCategories = Enum.GetNames<PageAccess>();

            return View(vm);
        }
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("ClaimTypes NameIdentifier is not found");

        Issue issue = new()
        {
            Category = vm.Category,
            SubCategory = vm.SubCategory,
            Note = vm.Note!,
            Status = IssueStatuses.Pending.ToString(),
        };

        await _context.Issues.AddAsync(issue, ct);
        await _context.SaveChangesAsync(ct);

        return RedirectToAction(nameof(Index));
    }

    // ================= Update =================
    [AuthorizePermission((int)Pages.Issues, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Update(int id, CancellationToken ct = default)
    {
        IssueUpdateVM vm = await _context.Issues
            .Where(i => !i.IsDeleted && i.Id == id)
            .Select(i => new IssueUpdateVM
            {
                Id = i.Id,
                Category = i.Category,
                SubCategory = i.SubCategory,
                Note = i.Note,
                Status = i.Status,
                Statuses = Enum.GetNames<IssueStatuses>(),
                Categories = Enum.GetNames<Pages>(),
                SubCategories = Enum.GetNames<PageAccess>(),
            })
            .FirstOrDefaultAsync(ct) ?? throw new Exception($"Issue is not found with this Id: {id}");

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthorizePermission((int)Pages.Issues, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Update(int id, IssueUpdateVM vm, CancellationToken ct = default)
    {
        if (id != vm.Id) return BadRequest(ct);

        if (!ModelState.IsValid)
        {
            vm.Statuses = Enum.GetNames<IssueStatuses>();
            vm.Categories = Enum.GetNames<Pages>();
            vm.SubCategories = Enum.GetNames<PageAccess>();
            return View(vm);
        }

        Issue issue = await _getIssueAsync(id, ct);

        issue.Category = vm.Category;
        issue.SubCategory = vm.SubCategory;
        issue.Note = vm.Note ?? "";
        issue.Status = vm.Status;
        
        issue.UpdatedTime = DateTime.UtcNow.AddHours(4);

        await _context.SaveChangesAsync(ct);

        return RedirectToAction(nameof(Index));
    }

    // ================= DELETE =================
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var data = await _context.Issues
    //        .Include(x => x.Category)
    //        .Where(x => x.Id == id && !x.IsDeleted)
    //        .Select(x => new IssueDetailsVM
    //        {
    //            Title = x.Title,
    //            Subtitle = x.Subtitle,
    //            CategoryName = x.Category != null ? x.Category.Name : null
    //        })
    //        .FirstOrDefaultAsync();

    //    if (data == null) return NotFound();

    //    return View(data);
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthorizePermission((int)Pages.Issues, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Delete(int id, string? returnUrl, CancellationToken ct = default)
    {
        var issue = await _getIssueAsync(id, ct, true);

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync(ct);
        Console.WriteLine(returnUrl);
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fromAll"> whether only from amoung all or only not deleted part</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task<Issue> _getIssueAsync(int id, CancellationToken ct = default, bool fromAll = false)
    {
        if (fromAll)
            return await _context.Issues.FindAsync(id, ct) ?? throw new Exception($"Issue is not found with this id: {id}");
        else
            return await _context.Issues.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, ct) ?? throw new Exception($"Issue is not found with this id: {id}");
    }
}

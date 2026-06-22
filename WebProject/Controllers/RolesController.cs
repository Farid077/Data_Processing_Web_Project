using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Attributes;
using WebProject.DataAccess;
using WebProject.Models;

namespace WebProject.Controllers;

[AuthorizePermission(Pages.Roles, PageAccess.Read)]
public class RolesController(WebProjectDbContext _context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .Include(r => r.Users)
            .ToListAsync(ct);

        return View(roles);
    }

    [AuthorizePermission(Pages.Roles, PageAccess.Create)]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Role { Permissions = [] });
    }

    [AuthorizePermission(Pages.Roles, PageAccess.Create)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string Name, CancellationToken ct = default)
    {
        Name = Name?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(Name))
        {
            ModelState.AddModelError(nameof(Name), "Rol adı zəruridir.");
            return View(new Role());
        }

        if (await _context.Roles.AnyAsync(r => r.Name.ToLower() == Name.ToLower(), ct))
        {
            ModelState.AddModelError(nameof(Name), "Bu rol artıq mövcuddur.");
            return View(new Role());
        }

        // Parse permissions from form data
        var permissions = new Dictionary<byte, int>();
        foreach (var page in Enum.GetValues<Pages>())
        {
            int accessValue = 0;
            foreach (var access in Enum.GetValues<PageAccess>())
            {
                //if (access == PageAccess.FullAccess) continue;

                var formKey = $"perms_{(int)page}_{(int)access}";
                if (Request.Form.ContainsKey(formKey))
                {
                    accessValue |= (int)access;
                }
            }
            permissions[(byte)page] = accessValue;
        }

        var role = new Role
        {
            Name = Name,
            Permissions = permissions
        };

        await _context.Roles.AddAsync(role, ct);
        await _context.SaveChangesAsync(ct);

        return RedirectToAction(nameof(Index));
    }

    [AuthorizePermission(Pages.Roles, PageAccess.Update)]
    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == id)
            ?? throw new Exception($"Bu ID ilə rol tapılmadı: {id}");

        return View(role);
    }

    [AuthorizePermission(Pages.Roles, PageAccess.Update)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string id, CancellationToken ct = default)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == id, ct)
            ?? throw new Exception($"Bu ID ilə rol tapılmadı: {id}");

        // Parse permissions from form data
        var permissions = new Dictionary<byte, int>();
        foreach (var page in Enum.GetValues<Pages>())
        {
            int accessValue = 0;
            foreach (var access in Enum.GetValues<PageAccess>())
            {
                //if (access == PageAccess.FullAccess) continue;

                var formKey = $"perms_{(int)page}_{(int)access}";
                if (Request.Form.ContainsKey(formKey))
                {
                    accessValue |= (int)access;
                }
            }
            permissions[(byte)page] = accessValue;
        }

        role.Permissions = permissions;
        await _context.SaveChangesAsync(ct);

        return RedirectToAction(nameof(Index));
    }

    [AuthorizePermission(Pages.Roles, PageAccess.Delete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id, CancellationToken ct = default)
    {
        var role = await _context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Name == id, ct)
            ?? throw new Exception($"Bu ID ilə rol tapılmadı: {id}");

        if (role.Name == "SuperAdmin" || role.Name == "User")
            return BadRequest("Sistem rollarını silmək olmaz.");

        foreach (var user in role.Users)
            user.RoleId = "User";

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(ct);

        return RedirectToAction(nameof(Index));
    }
}
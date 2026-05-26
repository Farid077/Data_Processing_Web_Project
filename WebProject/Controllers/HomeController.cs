using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebProject.Attributes;
using WebProject.DataAccess;
using WebProject.Models;
using WebProject.Services;
using WebProject.ViewModels;

namespace WebProject.Controllers;

//[AuthorizePermission((int)Pages.Dashboard, (int)PageAccess.Read)]
[Authorize]
public class HomeController : Controller
{
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View();
    }


    [Authorize]
    public IActionResult DepotData(int depot)
    {
        var permissions = User.FindAll("Permission").Select(perm => int.Parse(perm.Value));

        var page = Enum.GetValues<Pages>().FirstOrDefault(x => x.ToString() == "Depo" + depot);
        bool isAllowed = permissions.Any(perm => (perm & (int)page) == (int)page && (perm & (int)PageAccess.Read) == (int)PageAccess.Read);
        if (!isAllowed)
            return new ForbidResult();

        return View(depot);
    }
    

    [AuthorizePermission((int)Pages.AllDepos, (int)PageAccess.Read)]
    public IActionResult AllDepotsData() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage = "Error!")
    {
        return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = errorMessage ?? "Error!" });
    }

    //[HttpGet]
    //public IActionResult Export([FromQuery] GridRequest request)
    //{
    //    var data = _dataStore.GetData();
    //    if (data == null) return NotFound();

    //    // Apply same filters as GetData but no pagination
    //    var rows = data.Rows.AsEnumerable();

    //    if (!string.IsNullOrWhiteSpace(request.SearchTerm))
    //    {
    //        var term = request.SearchTerm.ToLower();
    //        rows = rows.Where(r => r.Cells.Values.Any(v => v.ToLower().Contains(term)));
    //    }

    //    foreach (var filter in request.ColumnFilters)
    //    {
    //        if (!string.IsNullOrWhiteSpace(filter.Value))
    //        {
    //            var col = filter.Key;
    //            var val = filter.Value.ToLower();
    //            rows = rows.Where(r => r.Cells.TryGetValue(col, out var cv) && cv.ToLower().Contains(val));
    //        }
    //    }

    //    if (!string.IsNullOrWhiteSpace(request.SortColumn))
    //    {
    //        rows = request.SortDirection == "desc"
    //            ? rows.OrderByDescending(r => r.Cells.TryGetValue(request.SortColumn, out var v) ? v : "")
    //            : rows.OrderBy(r => r.Cells.TryGetValue(request.SortColumn, out var v) ? v : "");
    //    }

    //    var bytes = _excelService.Export(data, rows.ToList());
    //    var fileName = $"export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
    //    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    //}


    //public IActionResult Index()
    //{
    //    ExcelData data = _dataStore.GetData() ?? new ExcelData();
    //    //ViewBag.HasData = data != null;
    //    //ViewBag.FileName = data?.FileName;
    //    //ViewBag.ImportedAt = data?.ImportedAt;
    //    //ViewBag.RowCount = data?.Rows.Count ?? 0;
    //    return View(data);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Upload(IFormFile file)
    //{
    //    if (file == null || file.Length == 0)
    //        return Json(new { success = false, message = "Please select a file." });

    //    var ext = Path.GetExtension(file.FileName).ToLower();
    //    if (ext != ".xlsx" && ext != ".xls")
    //        return Json(new { success = false, message = "Only .xlsx and .xls files are supported." });

    //    try
    //    {
    //        var data = await _excelService.ImportAsync(file);
    //        _dataStore.SetData(data);
    //        return Json(new { success = true, message = $"Imported {data.Rows.Count} rows with {data.Headers.Count} columns." });
    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(new { success = false, message = $"Error: {ex.Message}" });
    //    }
    //}

    //[HttpGet]
    //public IActionResult GetData([FromQuery] GridRequest request)
    //{
    //    var data = _dataStore.GetData();
    //    if (data == null)
    //        return Json(new GridResponse());

    //    var rows = data.Rows.AsEnumerable();

    //    // Global search
    //    if (!string.IsNullOrWhiteSpace(request.SearchTerm))
    //    {
    //        var term = request.SearchTerm.ToLower();
    //        rows = rows.Where(r => r.Cells.Values.Any(v => v.ToLower().Contains(term)));
    //    }

    //    // Column filters
    //    foreach (var filter in request.ColumnFilters)
    //    {
    //        if (!string.IsNullOrWhiteSpace(filter.Value))
    //        {
    //            var col = filter.Key;
    //            var val = filter.Value.ToLower();
    //            rows = rows.Where(r => r.Cells.TryGetValue(col, out var cv) && cv.ToLower().Contains(val));
    //        }
    //    }

    //    // Sort
    //    if (!string.IsNullOrWhiteSpace(request.SortColumn))
    //    {
    //        rows = request.SortDirection == "desc"
    //            ? rows.OrderByDescending(r => r.Cells.TryGetValue(request.SortColumn, out var v) ? v : "")
    //            : rows.OrderBy(r => r.Cells.TryGetValue(request.SortColumn, out var v) ? v : "");
    //    }

    //    var totalCount = rows.Count();
    //    var paged = rows
    //        .Skip((request.Page - 1) * request.PageSize)
    //        .Take(request.PageSize)
    //        .ToList();

    //    return RedirectToAction("Index");

    //    //return Json(new GridResponse
    //    //{
    //    //    Headers = data.Headers,
    //    //    Rows = paged,
    //    //    TotalCount = totalCount,
    //    //    Page = request.Page,
    //    //    PageSize = request.PageSize,
    //    //    TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
    //    //    FileName = data.FileName
    //    //});
    //}

    //[HttpPost]
    //public IActionResult EditRow([FromBody] EditRowRequest request)
    //{
    //    try
    //    {
    //        _dataStore.UpdateRow(request.RowId, request.Cells);
    //        return Json(new { success = true });
    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(new { success = false, message = ex.Message });
    //    }
    //}

    //[HttpPost]
    //public IActionResult DeleteRow([FromBody] int rowId)
    //{
    //    _dataStore.DeleteRow(rowId);
    //    return Json(new { success = true });
    //}

    //[HttpPost]
    //public IActionResult AddRow([FromBody] Dictionary<string, string> cells)
    //{
    //    _dataStore.AddRow(cells);
    //    return Json(new { success = true });
    //}


    //[HttpPost]
    //public IActionResult ClearData()
    //{
    //    _dataStore.SetData(new ExcelData());
    //    return Json(new { success = true });
    //}
}

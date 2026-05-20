using DocumentFormat.OpenXml.Math;
//using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security.Claims;
using WebProject.Attributes;
using WebProject.DataAccess;
using WebProject.Migrations;
using WebProject.Models;


namespace WebProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepotDataController(WebProjectDbContext _context) : ControllerBase
{
    // ── Replace these two static stores with your DbContext ──────────────
    //private static List<DepotData> _items = new()
    //{
    //    new DepotData { Id = 1, SN = 1, DQN = "DQN-001", EyNom = "Eye-1", DVR = "DVR-001", IsConfirmed = false },
    //    new DepotData { Id = 2, SN = 2, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
    //};

    // Persisted blocked columns: set of column names
    private static HashSet<string> _blockedColumns = new();
    // ─────────────────────────────────────────────────────────────────────

    // GET api/depotdata
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? depot, CancellationToken ct = default)
    {
        if (depot == null)
            return Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

        return Ok(await _context.DepotData.Where(x => !x.IsDeleted && x.Depot == depot).ToListAsync(ct));
    }

    //[HttpGet("1")]
    //public async Task<IActionResult> GetAll([FromQuery] int? depot, CancellationToken ct = default)
    //{
    //    if (depot == null)
    //        return Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

    //    return Ok(await _context.DepotData.Where(x => !x.IsDeleted && x.Depot == depot).ToListAsync(ct));
    //}
    //[HttpGet("2")]
    //public async Task<IActionResult> GetAll([FromQuery] int? depot, CancellationToken ct = default)
    //{
    //    if (depot == null)
    //        return Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

    //    return Ok(await _context.DepotData.Where(x => !x.IsDeleted && x.Depot == depot).ToListAsync(ct));
    //}
    //[HttpGet("3")]
    //public async Task<IActionResult> GetAll([FromQuery] int? depot, CancellationToken ct = default)
    //{
    //    if (depot == null)
    //        return Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

    //    return Ok(await _context.DepotData.Where(x => !x.IsDeleted && x.Depot == depot).ToListAsync(ct));
    //}
    //[HttpGet("4")]
    //public async Task<IActionResult> GetAll([FromQuery] int? depot, CancellationToken ct = default)
    //{
    //    if (depot == null)
    //        return Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

    //    return Ok(await _context.DepotData.Where(x => !x.IsDeleted && x.Depot == depot).ToListAsync(ct));
    //}
    //[HttpGet("5")]
    //public async Task<IActionResult> GetAll([FromQuery] int? depot, CancellationToken ct = default)
    //{
    //    if (depot == null)
    //        return Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

    //    return Ok(await _context.DepotData.Where(x => !x.IsDeleted && x.Depot == depot).ToListAsync(ct));
    //}

    // POST api/depotdata
    [HttpPost]
    [AuthorizePermission((int)Pages.AllDepos, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Create([FromBody] DepotData item, CancellationToken ct = default)
    {

        if (string.IsNullOrWhiteSpace(item.DQN)) return BadRequest("DQN is required.");
        //item.Id = _nextId++;
        //item.IsDeleted = false;
        //_items.Add(item);
        await _context.DepotData.AddAsync(item, ct);
        await _context.SaveChangesAsync(ct);
        return Ok(item);
    }

    // PUT api/depotdata/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepotData updated, CancellationToken ct = default)
    {
        //var item = _items.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        var data = _context.DepotData.FirstOrDefault(x => x.Id == id);
        //if (item == null) return NotFound();
        if (data == null) return NotFound();

        //var permissions = User.FindAll("Permissions");
        var permissions = User.FindAll("Permission").Select(perm => int.Parse(perm.Value));

        if (data.Depot == null)
        {
            bool isAllowed = permissions.Any(perm => (perm & (int)Pages.AllDepos) == (int)Pages.AllDepos && (perm & (int)PageAccess.Read_Write) == (int)PageAccess.Read_Write);
            if (!isAllowed)
                return BadRequest("You don't have permission to do this");
        }
        else
        {
            var page = Enum.GetValues<Pages>().FirstOrDefault(x => x.ToString() == "Depo" + data.Depot);
            bool isAllowed = permissions.Any(perm => (perm & (int)page) == (int)page && (perm & (int)PageAccess.Read_Write) == (int)PageAccess.Read_Write);
            if (!isAllowed)
                return BadRequest("You don't have permission to do this");
        }

        // Only update fields that are NOT blocked
        if (!_blockedColumns.Contains("SN"))       data.SN       = updated.SN;
        if (!_blockedColumns.Contains("DQN"))      data.DQN      = updated.DQN;
        if (!_blockedColumns.Contains("EyNom"))    data.EyNom    = updated.EyNom;
        if (!_blockedColumns.Contains("DVR"))      data.DVR      = updated.DVR;
        if (!_blockedColumns.Contains("CD"))       data.CD       = updated.CD;
        if (!_blockedColumns.Contains("QapiR"))    data.QapiR    = updated.QapiR;
        if (!_blockedColumns.Contains("SayKam"))   data.SayKam   = updated.SayKam;
        if (!_blockedColumns.Contains("HDDV"))    data.HDDV    = updated.HDDV;
        if (!_blockedColumns.Contains("HDDH"))    data.HDDH    = updated.HDDH;
        if (!_blockedColumns.Contains("HDDSM"))    data.HDDSM    = updated.HDDSM;
        if (!_blockedColumns.Contains("DVRV"))    data.DVRV    = updated.DVRV;
        if (!_blockedColumns.Contains("Kam"))      data.Kam      = updated.Kam;
        if (!_blockedColumns.Contains("KamV"))    data.KamV    = updated.KamV;
        if (!_blockedColumns.Contains("KamNom"))   data.KamNom   = updated.KamNom;
        if (!_blockedColumns.Contains("SalMon"))   data.SalMon   = updated.SalMon;
        if (!_blockedColumns.Contains("DaySes"))   data.DaySes   = updated.DaySes;
        if (!_blockedColumns.Contains("SurMik"))   data.SurMik   = updated.SurMik;
        if (!_blockedColumns.Contains("Trafared")) data.Trafared = updated.Trafared;
        if (!_blockedColumns.Contains("Qeyd"))     data.Qeyd     = updated.Qeyd;

        data.UpdatedTime = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(4), DateTimeKind.Utc);
        await _context.SaveChangesAsync(ct);

        return Ok(data);
    }

    // DELETE api/depotdata/{id}  (soft delete)
    [HttpDelete("{id}")]
    [AuthorizePermission((int)Pages.AllDepos, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        //var item = _items.FirstOrDefault(x => x.Id == id);
        var data = await _context.DepotData.FirstOrDefaultAsync(x => x.Id == id, ct);
        //if (item == null) return NotFound();
        if (data == null) return NotFound();
        //item.IsDeleted = true;
        data.IsDeleted = true;
        await _context.SaveChangesAsync(ct);
        return Ok();
    }

    // DELETE api/depotdata/All  (hard delete)
    [HttpDelete("All")]
    [AuthorizePermission((int)Pages.AllDepos, (int)PageAccess.Read_Write)]
    public async Task<IActionResult> Delete(CancellationToken ct = default)
    {
        //var item = _items.FirstOrDefault(x => x.Id == id);
        //var data = await _context.DepotData.FirstOrDefaultAsync(x => x.Id == id, ct);
        //if (item == null) return NotFound();
        //if (data == null) return NotFound();
        //item.IsDeleted = true;
        //data.IsDeleted = true;
        await _context.DepotData.ExecuteDeleteAsync(ct);
        //await _context.SaveChangesAsync(ct);
        return Ok();
    }

    // Confirm api/depotdata/Confirm/{id}
    [HttpPost("Confirm/{id}")]
    public async Task<IActionResult> Confirm(int id, CancellationToken ct = default)
    {
        //var item = _items.FirstOrDefault(x => x.Id == id);
        var data = await _context.DepotData.FirstOrDefaultAsync(x => x.Id == id, ct);
        //if (item == null) return NotFound();
        if (data == null) return NotFound();

        if (HttpContext.User.Identity?.IsAuthenticated == true)
            data.ConfirmerId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("ClaimTypes NameIdentifier is not found");
        else
            throw new Exception("HttpContext.User.Identity?.IsAuthenticated");
        
        //item.IsConfirmed = true;
        data.IsConfirmed = true;
        data.ConfirmedDate = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(4), DateTimeKind.Utc);
        await _context.SaveChangesAsync(ct);
        return Ok(data);
    }

    // GET api/depotdata/blocked-columns
    [HttpGet("blocked-columns")]
    public IActionResult GetBlockedColumns() => Ok(_blockedColumns);

    // POST api/depotdata/blocked-columns/{column}  — toggle
    [HttpPost("blocked-columns/{column}")]
    public IActionResult ToggleBlock(string column)
    {
        if (_blockedColumns.Contains(column))
            _blockedColumns.Remove(column);
        else
            _blockedColumns.Add(column);
        return Ok(_blockedColumns);
    }


    [HttpPost("import")]
    public async Task<IActionResult> Import(int? depot, IFormFile file, CancellationToken ct = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only .xlsx files are supported.");

        ExcelPackage.License.SetNonCommercialPersonal("MyApp");

        using var stream = file.OpenReadStream();
        using var package = new ExcelPackage(stream);

        //var sheet = package.Workbook.Worksheets[0];
        //int rows = sheet.Dimension?.Rows ?? 0;
        //int cols = sheet.Dimension?.Columns ?? 0;

        //if (rows < 2) return BadRequest("File is empty or has no data rows.");

        //// First row = headers
        //var headers = Enumerable.Range(1, cols)
        //    .Select(c => sheet.Cells[1, c].Text?.Trim() ?? $"Column{c}")
        //    .ToList();

        //// Each data row becomes a dictionary of { header → value }
        //var results = new List<Dictionary<string, string>>();

        //for (int row = 2; row <= rows; row++)
        //{
        //    var dict = new Dictionary<string, string>();
        //    bool isEmpty = true;

        //    for (int col = 1; col <= cols; col++)
        //    {
        //        var val = sheet.Cells[row, col].Text?.Trim() ?? "";
        //        dict[headers[col - 1]] = val;
        //        if (!string.IsNullOrWhiteSpace(val)) isEmpty = false;
        //    }

        //    if (!isEmpty) results.Add(dict);
        //}

        // ── Map to DepotData and save to DB here ──
        // foreach (var row in results)
        // {
        //     var item = new DepotData { DQN = row["YourColumnName"], ... };
        //     _context.DepotDatas.Add(item);
        // }
        // await _context.SaveChangesAsync();

        var results = new List<DepotData>();
        int depotNum = depot ?? 0;
        var sheet = package.Workbook.Worksheets[0]; // first sheet

        if (package.Workbook.Worksheets.Count() >= depotNum)
        {
            sheet = package.Workbook.Worksheets[depotNum-1];
        }
        //var sheet = package.Workbook.Worksheets.Count(); // first sheet
        int rows = sheet.Dimension?.Rows ?? 0;

        if (rows < 2) return BadRequest("Excel file is empty or has no data rows.");

        // Read header row to map column names (case-insensitive)
        var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (int col = 1; col <= sheet.Dimension?.Columns; col++)
        {
            var header = sheet.Cells[1, col].Text?.Trim();
            if (!string.IsNullOrEmpty(header))
                headers[header] = col;
        }

        string Get(int row, string colName) =>
            headers.TryGetValue(colName, out int c)
                ? sheet.Cells[row, c].Text?.Trim() ?? ""
                : "";

        for (int row = 2; row <= rows; row++)
        {
            // Skip completely empty rows
            bool isEmpty = true;
            for (int c = 1; c <= sheet.Dimension?.Columns; c++)
            {
                if (!string.IsNullOrWhiteSpace(sheet.Cells[row, c].Text)) { isEmpty = false; break; }
            }
            if (isEmpty) continue;

            var item = new DepotData
            {
                SN = int.TryParse(Get(row, "SN"), out int sn) ? sn : 0,
                DQN = Get(row, "DQN"),
                EyNom = Get(row, "EyNom"),
                DVR = Get(row, "DVR"),
                CD = Get(row, "CD"),
                QapiR = Get(row, "QapiR"),
                SayKam = Get(row, "SayKam"),
                HDDV = Get(row, "HDDSt"),
                HDDH = Get(row, "HDDHc"),
                HDDSM = Get(row, "HDDSM"),
                DVRV = Get(row, "DVRSt"),
                Kam = Get(row, "Kam"),
                KamV = Get(row, "KamSt"),
                KamNom = Get(row, "KamNom"),
                SalMon = Get(row, "SalMon"),
                DaySes = Get(row, "DaySes"),
                SurMik = Get(row, "SurMik"),
                Trafared = Get(row, "Trafared"),
                Qeyd = Get(row, "Qeyd"),
                Depot = depot,
            };

            results.Add(item);
        }

        // ── Do your DB save here ──────────────────────────────────
        await _context.DepotData.AddRangeAsync(results, ct);
        await _context.SaveChangesAsync(ct);
        // ─────────────────────────────────────────────────────────

        //return Ok(new { imported = results.Count, rows = results });
        return RedirectToAction("Index", "Home");
    }
}

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

        var blockedRows = await _context.BlockLists.FirstOrDefaultAsync(x => x.Key == "Rows") ?? throw new Exception("Blocked rows list is not found.");

        if (blockedRows.Value.Contains(id.ToString()))
        {
            return BadRequest("This row is blocked.");
        }

        var blockedColumns = await _context.BlockLists.FirstOrDefaultAsync(x => x.Key == "Columns") ?? throw new Exception("Blocked columns list is not found.");
        ICollection<string> blockedColumnsList = blockedColumns.Value;

        // Only update fields that are NOT blocked
        if (!blockedColumnsList.Contains("SN"))       data.SN       = updated.SN;
        if (!blockedColumnsList.Contains("DQN"))      data.DQN      = updated.DQN;
        if (!blockedColumnsList.Contains("EyNom"))    data.EyNom    = updated.EyNom;
        if (!blockedColumnsList.Contains("DVR"))      data.DVR      = updated.DVR;
        if (!blockedColumnsList.Contains("CD"))       data.CD       = updated.CD;
        if (!blockedColumnsList.Contains("QapiR"))    data.QapiR    = updated.QapiR;
        if (!blockedColumnsList.Contains("SayKam"))   data.SayKam   = updated.SayKam;
        if (!blockedColumnsList.Contains("HDDV"))     data.HDDV     = updated.HDDV;
        if (!blockedColumnsList.Contains("HDDH"))     data.HDDH     = updated.HDDH;
        if (!blockedColumnsList.Contains("HDDSM"))    data.HDDSM    = updated.HDDSM;
        if (!blockedColumnsList.Contains("DVRV"))     data.DVRV     = updated.DVRV;
        if (!blockedColumnsList.Contains("Kam"))      data.Kam      = updated.Kam;
        if (!blockedColumnsList.Contains("KamV"))     data.KamV     = updated.KamV;
        if (!blockedColumnsList.Contains("KamNom"))   data.KamNom   = updated.KamNom;
        if (!blockedColumnsList.Contains("SalMon"))   data.SalMon   = updated.SalMon;
        if (!blockedColumnsList.Contains("DaySes"))   data.DaySes   = updated.DaySes;
        if (!blockedColumnsList.Contains("SurMik"))   data.SurMik   = updated.SurMik;
        if (!blockedColumnsList.Contains("Trafared")) data.Trafared = updated.Trafared;
        if (!blockedColumnsList.Contains("Qeyd"))     data.Qeyd     = updated.Qeyd;

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
    public async Task<IActionResult> GetBlockedColumns()
    {
        var blockedColumns = await _context.BlockLists.FirstOrDefaultAsync(x => x.Key == "Columns") ?? throw new Exception("Blocked columns list is not found.");

        return Ok(blockedColumns.Value);
    }

    // POST api/depotdata/blocked-columns/{column}  — toggle
    [HttpPost("blocked-columns/{column}")]
    public async Task<IActionResult> ToggleBlock(string column)
    {
        if (_blockedColumns.Contains(column))
            _blockedColumns.Remove(column);
        else
            _blockedColumns.Add(column);

        var blockedColumns = await _context.BlockLists.FirstOrDefaultAsync(x => x.Key == "Columns") ?? throw new Exception("Blocked columns list is not found.");

        if (!blockedColumns.Value.Remove(column))
            blockedColumns.Value.Add(column);
        await _context.SaveChangesAsync();

        return Ok(blockedColumns.Value);
    }


    // POST api/depotdata/block-row/{id}  — toggle
    [HttpPost("block-row/{id}")]
    public async Task<IActionResult> ToggleBlockRow(int id)
    {
        var blockedRows = await _context.BlockLists.FirstOrDefaultAsync(x => x.Key == "Rows") ?? throw new Exception("Blocked rows list is not found.");

        if (!blockedRows.Value.Remove(id.ToString()))
            blockedRows.Value.Add(id.ToString());

        return Ok(blockedRows.Value);
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


        var results = new List<DepotData>();
        int depotNum = depot ?? 0;
        var sheet = package.Workbook.Worksheets[0]; // first sheet

        if (package.Workbook.Worksheets.Count() >= depotNum)
        {
            sheet = package.Workbook.Worksheets[depotNum-1];
        }

        int rows = sheet.Dimension?.Rows ?? 0;

        if (rows < 2) return BadRequest("Excel file is empty or has no data rows.");

        // Read header row to map column names (case-insensitive)
        var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (int col = 1; col <= sheet.Dimension?.Columns; col++)
        {
            var header = sheet.Cells[1, col].Text.Trim();
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
                HDDV = Get(row, "HddV"),
                HDDH = Get(row, "HddH"),
                HDDSM = Get(row, "HddSM"),
                DVRV = Get(row, "DvrV"),
                Kam = Get(row, "Kam"),
                KamV = Get(row, "KamV"),
                KamNom = Get(row, "KamNom"),
                SalMon = Get(row, "SalMon"),
                DaySes = Get(row, "DaySes"),
                SurMik = Get(row, "SurMik"),
                Trafared = Get(row, "Trafared"),
                Qeyd = Get(row, "Qeyd"),
                Depot = depot,
                //ConfirmerId = Get(row, "")
            };

            results.Add(item);
        }

        await _context.DepotData.AddRangeAsync(results, ct);
        await _context.SaveChangesAsync(ct);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("export")]
public IActionResult Export([FromBody] List<DepotData> rows)
{
    ExcelPackage.License.SetNonCommercialPersonal("MyApp");

    using var package = new ExcelPackage();
    var sheet = package.Workbook.Worksheets.Add("DepotData");

    // Excluded columns
    var excluded = new HashSet<string> { "Id", "User", "IsDeleted", "IsConfirmed", "CreatedTime", "Depot", };

    var props = typeof(DepotData).GetProperties()
        .Where(p => !excluded.Contains(p.Name))
        .ToList();

    // ── Header row ────────────────────────────────────────────
    for (int i = 0; i < props.Count; i++)
    {
        var cell = sheet.Cells[1, i + 1];
        cell.Value = props[i].Name;

        // Light blue background
        cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(189, 215, 238));

        // Bold text
        cell.Style.Font.Bold = true;
        cell.Style.Font.Size = 11;

        // Center align
        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        cell.Style.VerticalAlignment   = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        // Border
        cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        cell.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(100, 149, 190));
    }

    // Make header row tall (≈ 3-4 normal rows)
    sheet.Row(1).Height = 45;

    // ── Data rows ─────────────────────────────────────────────
    for (int r = 0; r < rows.Count; r++)
        for (int c = 0; c < props.Count; c++)
            sheet.Cells[r + 2, c + 1].Value = props[c].GetValue(rows[r])?.ToString() ?? "";

    sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

    var bytes = package.GetAsByteArray();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"DepotData_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
}
}

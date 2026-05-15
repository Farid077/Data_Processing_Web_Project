using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebProject.DataAccess;
using WebProject.Models;


namespace WebProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepotDataController(WebProjectDbContext _context) : ControllerBase
{
    // ── Replace these two static stores with your DbContext ──────────────
    private static List<DepotData> _items = new()
    {
        new DepotData { Id = 1, SN = 1, DQN = "DQN-001", EyNom = "Eye-1", DVR = "DVR-001", IsConfirmed = false },
        new DepotData { Id = 2, SN = 2, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 3, SN = 3, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 4, SN = 4, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 5, SN = 5, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 6, SN = 6, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 7, SN = 7, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 8, SN = 8, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 9, SN = 9, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 10, SN = 10, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 11, SN = 11, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 12, SN = 12, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 13, SN = 13, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 14, SN = 14, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 15, SN = 15, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 16, SN = 16, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 17, SN = 17, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 18, SN = 18, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 19, SN = 19, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 20, SN = 20, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 21, SN = 21, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        new DepotData { Id = 22, SN = 22, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
    };
    private static int _nextId = 23;

    // Persisted blocked columns: set of column names
    private static HashSet<string> _blockedColumns = new();
    // ─────────────────────────────────────────────────────────────────────

    // GET api/depotdata
    [HttpGet]
    public async Task<IActionResult> GetAll(int? depot, CancellationToken ct = default) =>
        Ok(await _context.DepotData.Where(x => !x.IsDeleted).ToListAsync(ct));

    // POST api/depotdata
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepotData item, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(item.DQN)) return BadRequest("DQN is required.");
        //item.Id = _nextId++;
        //item.IsDeleted = false;
        _items.Add(item);
        await _context.DepotData.AddAsync(item, ct);
        await _context.SaveChangesAsync(ct);
        return Ok(item);
    }

    // PUT api/depotdata/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepotData updated, CancellationToken ct = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        var data = _context.DepotData.FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        if (data == null) return NotFound();

        // Only update fields that are NOT blocked
        if (!_blockedColumns.Contains("SN"))       data.SN       = updated.SN;
        if (!_blockedColumns.Contains("DQN"))      data.DQN      = updated.DQN;
        if (!_blockedColumns.Contains("EyNom"))    data.EyNom    = updated.EyNom;
        if (!_blockedColumns.Contains("DVR"))      data.DVR      = updated.DVR;
        if (!_blockedColumns.Contains("CD"))       data.CD       = updated.CD;
        if (!_blockedColumns.Contains("QapiR"))    data.QapiR    = updated.QapiR;
        if (!_blockedColumns.Contains("SayKam"))   data.SayKam   = updated.SayKam;
        if (!_blockedColumns.Contains("HDDSt"))    data.HDDSt    = updated.HDDSt;
        if (!_blockedColumns.Contains("HDDHc"))    data.HDDHc    = updated.HDDHc;
        if (!_blockedColumns.Contains("HDDSM"))    data.HDDSM    = updated.HDDSM;
        if (!_blockedColumns.Contains("DVRSt"))    data.DVRSt    = updated.DVRSt;
        if (!_blockedColumns.Contains("Kam"))      data.Kam      = updated.Kam;
        if (!_blockedColumns.Contains("KamSt"))    data.KamSt    = updated.KamSt;
        if (!_blockedColumns.Contains("KamNom"))   data.KamNom   = updated.KamNom;
        if (!_blockedColumns.Contains("SalMon"))   data.SalMon   = updated.SalMon;
        if (!_blockedColumns.Contains("DaySes"))   data.DaySes   = updated.DaySes;
        if (!_blockedColumns.Contains("SurMik"))   data.SurMik   = updated.SurMik;
        if (!_blockedColumns.Contains("Trafared")) data.Trafared = updated.Trafared;
        if (!_blockedColumns.Contains("Note"))     data.Note     = updated.Note;
        if (!_blockedColumns.Contains("ConfirmedDate")) data.ConfirmedDate = updated.ConfirmedDate;
        if (!_blockedColumns.Contains("IsConfirmed"))   data.IsConfirmed   = updated.IsConfirmed;

        data.UpdatedTime = DateTime.UtcNow.AddHours(4);
        await _context.SaveChangesAsync(ct);

        return Ok(data);
    }

    // DELETE api/depotdata/{id}  (soft delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        var data = await _context.DepotData.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item == null) return NotFound();
        if (data == null) return NotFound();
        item.IsDeleted = true;
        data.IsDeleted = true;
        await _context.SaveChangesAsync(ct);
        return Ok();
    }

    // Confirm api/depotdata/Confirm/{id}
    [HttpPost("Confirm/{id}")]
    public async Task<IActionResult> Confirm(int id, CancellationToken ct = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        var data = await _context.DepotData.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item == null) return NotFound();
        if (data == null) return NotFound();

        if (HttpContext.User.Identity?.IsAuthenticated == true)
            data.ApproverId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("ClaimTypes NameIdentifier is not found");
        else
            throw new Exception("ClaimTypes NameIdentifier is not found");
        
        item.IsConfirmed = true;
        data.IsConfirmed = true;
        data.ConfirmedDate = DateTime.UtcNow.AddHours(4);
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
}

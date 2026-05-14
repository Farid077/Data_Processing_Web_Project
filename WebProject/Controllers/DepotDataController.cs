using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetAll() =>
        Ok(_items.Where(x => !x.IsDeleted).ToList());

    // POST api/depotdata
    [HttpPost]
    public IActionResult Create([FromBody] DepotData item)
    {
        if (string.IsNullOrWhiteSpace(item.DQN)) return BadRequest("DQN is required.");
        item.Id = _nextId++;
        item.IsDeleted = false;
        _items.Add(item);
        return Ok(item);
    }

    // PUT api/depotdata/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] DepotData updated)
    {
        var item = _items.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item == null) return NotFound();

        // Only update fields that are NOT blocked
        if (!_blockedColumns.Contains("SN"))       item.SN       = updated.SN;
        if (!_blockedColumns.Contains("DQN"))      item.DQN      = updated.DQN;
        if (!_blockedColumns.Contains("EyNom"))    item.EyNom    = updated.EyNom;
        if (!_blockedColumns.Contains("DVR"))      item.DVR      = updated.DVR;
        if (!_blockedColumns.Contains("CD"))       item.CD       = updated.CD;
        if (!_blockedColumns.Contains("QapiR"))    item.QapiR    = updated.QapiR;
        if (!_blockedColumns.Contains("SayKam"))   item.SayKam   = updated.SayKam;
        if (!_blockedColumns.Contains("HDDSt"))    item.HDDSt    = updated.HDDSt;
        if (!_blockedColumns.Contains("HDDHc"))    item.HDDHc    = updated.HDDHc;
        if (!_blockedColumns.Contains("HDDSM"))    item.HDDSM    = updated.HDDSM;
        if (!_blockedColumns.Contains("DVRSt"))    item.DVRSt    = updated.DVRSt;
        if (!_blockedColumns.Contains("Kam"))      item.Kam      = updated.Kam;
        if (!_blockedColumns.Contains("KamSt"))    item.KamSt    = updated.KamSt;
        if (!_blockedColumns.Contains("KamNom"))   item.KamNom   = updated.KamNom;
        if (!_blockedColumns.Contains("SalMon"))   item.SalMon   = updated.SalMon;
        if (!_blockedColumns.Contains("DaySes"))   item.DaySes   = updated.DaySes;
        if (!_blockedColumns.Contains("SurMik"))   item.SurMik   = updated.SurMik;
        if (!_blockedColumns.Contains("Trafared")) item.Trafared = updated.Trafared;
        if (!_blockedColumns.Contains("Note"))     item.Note     = updated.Note;
        if (!_blockedColumns.Contains("ConfirmedDate")) item.ConfirmedDate = updated.ConfirmedDate;
        if (!_blockedColumns.Contains("IsConfirmed"))   item.IsConfirmed   = updated.IsConfirmed;

        return Ok(item);
    }

    // DELETE api/depotdata/{id}  (soft delete)
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        item.IsDeleted = true;
        return Ok();
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

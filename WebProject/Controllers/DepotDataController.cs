using Microsoft.AspNetCore.Mvc;
using WebProject.Models;
// Make sure to add your DbContext using statement here
// using WebProject.Data;

namespace WebProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepotDataController : ControllerBase
    {
        // ── Replace these two static stores with your DbContext ──────────────
        private static List<DepotData> _items = new()
        {
            new DepotData { Id = 1, SN = 1, DQN = "DQN-001", EyNom = "Eye-1", DVR = "DVR-001", IsConfirmed = false },
            new DepotData { Id = 2, SN = 2, DQN = "DQN-002", EyNom = "Eye-2", DVR = "DVR-002", IsConfirmed = true, ConfirmedDate = DateTime.Today },
        };
        private static int _nextId = 3;

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
}

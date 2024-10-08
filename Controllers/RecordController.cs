using MedGETA.Context;
using MedGETA.DTO.Record;
using MedGETA.Records;
using Microsoft.AspNetCore.Mvc;

namespace MedGETA.Controllers
{
    public class RecordsController : ControllerBase
    {
        private readonly MedGETADbContext _context;

        public RecordsController(MedGETADbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllRecords")]
        public ActionResult<IEnumerable<RecordModel>> GetAllRecords()
        {
            List<Record>? records = _context.Records?.ToList();

            if (records is null)
            {
                return NotFound("No Records were found.");
            }

            List<RecordModel> recordsDTO = records.Select(r => new RecordModel
            {
                Id = r.Id,
                Diagnostic = r.Diagnostic
            }).ToList();

            return recordsDTO;
        }

        [HttpGet("findRecord/{id:guid}", Name="GetRecordById")]
        public ActionResult<RecordModel> GetRecordById(Guid id)
        {
            Record? record = _context.Records?.FirstOrDefault(h => h.Id == id);

            if (record is null)
            {
                return NotFound("No hospital found with this Id.");
            }

            RecordModel recordDTO = new()
            {
                Id = record.Id,
                Diagnostic = record.Diagnostic
            };

            return recordDTO;
        }

        [HttpPost("registerNewRecord")]
        public ActionResult<Record> CreateNewRecord([FromBody] Record? _Record)
        {
            if (_Record is null)
            {
                return BadRequest("Incomplete or wrong data, try again.");
            }

            _Record.GenerateUUID();

            _context.Records?.Add(_Record);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetRecordById",
            new { id = _Record.Id, _Record});
        }

        [HttpDelete("DeleteRecord")]
        public ActionResult<Record> DeleteRecord(Guid id)
        {
            Record? RecordToBeDeleted = _context.Records?.FirstOrDefault(h => h.Id == id);

            if (RecordToBeDeleted is null)
            {
                return NotFound("No Record with this Id was found.");
            }

            _context.Remove(RecordToBeDeleted);

            _context.SaveChanges();

            return Ok("Record deleted succesfully!");
        }
    }
}
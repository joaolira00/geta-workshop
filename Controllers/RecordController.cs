using MedGETA.Context;
using MedGETA.DTO;
using MedGETA.DTO.Record;
using MedGETA.Hospitals;
using MedGETA.Patients;
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

            if (records == null)
            {
                return NotFound("No Records were found.");
            }

            List<RecordModel> recordModels = records.Select(r => new RecordModel
            {
                Id = r.Id,
                Description = r.Description,
                PatientName = r.PatientName,
                HospitalName = r.HospitalName,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            return Ok(recordModels);
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
                Description = record.Description
            };

            return recordDTO;
        }

        [HttpPost("registerNewRecord")]
        public ActionResult<Record> CreateNewRecord([FromBody] RecordDto recordDTO)
        {
            if (recordDTO is null || recordDTO.PatientId == Guid.Empty || recordDTO.HospitalId == Guid.Empty)
            {
                return BadRequest("Incomplete or wrong data, provide the patient and hospital Ids and try again.");
            }

            Hospital? _hospital = _context.Hospitals?.FirstOrDefault(h => h.Id == recordDTO.HospitalId);
            Patient? _patient = _context.Patients?.FirstOrDefault(p => p.Id == recordDTO.PatientId);

            if (_hospital is null)
            {
                return NotFound("No hospitals found with this Id.");
            }


            if (_patient is null)
            {
                return NotFound("No patients found with this Id.");
            }

            Record newRecord = new()
            {
                Id = Guid.NewGuid(),
                Description = recordDTO.Description,
                PatientId = recordDTO.PatientId,
                PatientName = _patient.Name,
                HospitalId = recordDTO.HospitalId,
                HospitalName = _hospital.Name,
                CreatedAt = DateTime.UtcNow
            };
            

            _context.Records?.Add(newRecord);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetRecordById",
            new { id = newRecord.Id}, newRecord);
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
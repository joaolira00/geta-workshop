using MedGETA.Context;
using MedGETA.DTO.Record;
using MedGETA.Hospitals;
using MedGETA.Patients;
using MedGETA.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedGETA.Controllers
{
    public class HospitalRecordsController : ControllerBase
    {
        private readonly MedGETADbContext _context;

        public HospitalRecordsController(MedGETADbContext context)
        {
            _context = context;
        }

        [HttpGet("GetRecordInHospital/{hospitalName}")]
        public ActionResult<RecordModel> GetAllRecordsInHospital(string hospitalName)
        {
            Hospital? _hospital = _context.Hospitals?
                                            .Include(h => h.Records)
                                            .FirstOrDefault(h => h.Name == hospitalName);

            if (_hospital == null)
            {
                return NotFound("No hospital found with that name.");
            }

            List<RecordModel>? _records = _hospital.Records?.Select(r => new RecordModel
            {
                Id = r.Id,
                HospitalName = r.HospitalName,
                PatientName = r.PatientName,
                Description = r.Description,
                CreatedAt = r.CreatedAt
            }).ToList();

            return Ok(_records);
        }

        [HttpGet("GetRecordsFrom/{hospitalName}/{patientName}")]
        public ActionResult<RecordModel> GetRecordsFromPatient(string hospitalName, string patientName)
        {
            Hospital? _hospital = _context.Hospitals?
                                            .Include(h => h.Patients)
                                            .Include(h => h.Records)
                                            .FirstOrDefault(h => h.Name!.ToLower() == patientName.ToLower());

            if (_hospital == null)
            {
                return NotFound("No hospital with that name was found.");
            }

            Patient? _patient = _hospital.Patients?.FirstOrDefault(p => p.Name == patientName);

            if (_patient == null)
            {
                return NotFound("Wrong patient name or patient not registered in this hospital.");
            }

            List<RecordModel>? _records = _hospital.Records?
                                            .Where(r => r.PatientId == _patient.Id)
                                            .Select(r => new RecordModel
                                            {
                                                Id = r.Id,
                                                HospitalName = r.HospitalName,
                                                PatientName = r.PatientName,
                                                Description = r.Description,
                                                CreatedAt = r.CreatedAt
                                            }).ToList();

            if (_records == null || _records.Count == 0)
            {
                return NotFound("No records from this patient found in this hospital.");
            }

            return Ok(_records);
        }
    }
}
using MedGETA.Context;
using MedGETA.DTO.Hospital;
using MedGETA.DTO.Patient;
using MedGETA.Hospitals;
using MedGETA.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedGETA.Controllers
{
    public class HospitalController : ControllerBase
    {
        private readonly MedGETADbContext _context;

        public HospitalController(MedGETADbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllHospitals")]
        public ActionResult<IEnumerable<HospitalModel>> GetAllHospitals()
        {
            List<Hospital>? hospitals = _context.Hospitals?.Take(10).ToList();

            if (hospitals is null)
            {
                return NotFound("No hospitals were found.");
            }

            List<HospitalModel> hospitalsDTO = hospitals.Select(h => new HospitalModel
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address
            }).ToList();

            return hospitalsDTO;
        }

        [HttpGet("findHospital/{id:guid}", Name="GetHospitalById")]
        public ActionResult<HospitalModel> GetHospitalById(Guid id)
        {
            Hospital? hospital = _context.Hospitals?.FirstOrDefault(h => h.Id == id);

            if (hospital is null)
            {
                return NotFound("No hospital found with this Id.");
            }

            HospitalModel hospitalDTO = new()
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = hospital.Address
            };

            return hospitalDTO;
        }

        [HttpGet("listAllPatientsInHospital/{id}")]
        public ActionResult<PatientModel> GetPatientsInHospital(Guid id)
        {
            Hospital? hospital = _context.Hospitals?
                                            .Include(h => h.Patients)
                                            .FirstOrDefault(h => h.Id == id);

            if (hospital == null)
            {
                return NotFound("No hospital found with this Id.");
            }

            List<PatientModel>? patientDTO = hospital.Patients?.Select(p => new PatientModel
            {
                Id = p.Id,
                Name = p.Name,
                Email = p.Email
            }).ToList();

            return Ok(patientDTO);
        }

        [HttpPost("AddPatientToHospital/{hospitalName}")]
        public ActionResult<Patient> AddPatientToHospital(string hospitalName, [FromBody] List<string> patientList)
        {
            Hospital? _hospital = _context.Hospitals?
                                            .Include(h => h.Patients)
                                            .FirstOrDefault(h => h.Name == hospitalName);

            if (_hospital is null)
            {
                return BadRequest("Incomplete information about the patient, try again.");
            }

            List<Patient>? _patientToAdd = _context.Patients?.Where(p => patientList.Contains(p.Name!)).ToList();

            if (_patientToAdd is null)
            {
                return NotFound("No patient selected.");
            }

            foreach(Patient patient in _patientToAdd)
            {
                _hospital.Patients?.Add(patient);
            }

            _context.SaveChanges();

            return Ok("Patient registered succesfully.");

        }

        [HttpPost("createNewHospital")]
        public ActionResult<Hospital> CreateNewHospital([FromBody] Hospital? _hospital)
        {
            if (_hospital is null)
            {
                return BadRequest("Incomplete or wrong data, try again.");
            }

            _hospital.GenerateUUID();

            _context.Hospitals?.Add(_hospital);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetHospitalById",
            new { id = _hospital.Id, _hospital});
        }

        [HttpDelete("DeleteHospital")]
        public ActionResult<Hospital> DeleteHospital(Guid id)
        {
            Hospital? hospitalToBeDeleted = _context.Hospitals?.FirstOrDefault(h => h.Id == id);

            if (hospitalToBeDeleted is null)
            {
                return NotFound("No hospital with this Id was found.");
            }

            _context.Remove(hospitalToBeDeleted);

            _context.SaveChanges();

            return Ok("Hospital deleted succesfully!");
        }

        [HttpDelete("RemoveFromHospital/{hospitalName}")]
        public ActionResult<Patient> RemoveFromHospital(string hospitalName, [FromBody] List<Guid> patientList)
        {
            Hospital? _hospital = _context.Hospitals?
                                        .Include(h => h.Patients)
                                        .FirstOrDefault(h => h.Name == hospitalName);

            if (_hospital is null)
            {
                return NotFound("No hospitals found with this Id.");
            }

            List<Patient>? patientsToBeDeleted = _hospital?.Patients?.Where(p => patientList.Contains(p.Id)).ToList();

            if (patientsToBeDeleted is null || patientsToBeDeleted.Count == 0)
            {
                return NotFound("No patients found for removal.");
            }

            foreach(Patient patient in patientsToBeDeleted)
            {
                _hospital?.Patients?.Remove(patient);
            }

            _context.SaveChanges();

            return Ok(patientsToBeDeleted);
        }
    }
}
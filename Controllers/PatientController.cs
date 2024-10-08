using MedGETA.Context;
using MedGETA.DTO.Patient;
using MedGETA.Patients;
using Microsoft.AspNetCore.Mvc;

namespace MedGETA.Controllers
{
    public class PatientController : ControllerBase
    {
        private readonly MedGETADbContext _context;

        public PatientController(MedGETADbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllPatients")]
        public ActionResult<IEnumerable<PatientModel>> GetAllPatients()
        {
            List<Patient>? patients = _context.Patients?.ToList();

            if (patients is null)
            {
                return NotFound("No patients were found.");
            }

            List<PatientModel> patientsDTO = patients.Select(p => new PatientModel
            {
                Id = p.Id,
                Name = p.Name,
                Email = p.Email
            }).ToList();

            return patientsDTO;
        }

        [HttpGet("findPatient/{id:guid}", Name="GetPatientById")]
        public ActionResult<PatientModel> GetPatientById(Guid id)
        {
            Patient? patient = _context.Patients?.FirstOrDefault(h => h.Id == id);

            if (patient is null)
            {
                return NotFound("No hospital found with this Id.");
            }

            PatientModel patientDTO = new()
            {
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email
            };

            return patientDTO;
        }

        [HttpPost("registerNewPatient")]
        public ActionResult<Patient> CreateNewPatient([FromBody] Patient? _patient)
        {
            if (_patient is null)
            {
                return BadRequest("Incomplete or wrong data, try again.");
            }

            _patient.GenerateUUID();

            _context.Patients?.Add(_patient);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetPatientById",
            new { id = _patient.Id, _patient});
        }

        [HttpDelete("DeletePatient")]
        public ActionResult<Patient> DeletePatient(Guid id)
        {
            Patient? PatientToBeDeleted = _context.Patients?.FirstOrDefault(h => h.Id == id);

            if (PatientToBeDeleted is null)
            {
                return NotFound("No Patient with this Id was found.");
            }

            _context.Remove(PatientToBeDeleted);

            _context.SaveChanges();

            return Ok("Patient deleted succesfully!");
        }
    }
}
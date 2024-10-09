using MedGETA.Context;
using MedGETA.DTO.Hospital;
using MedGETA.DTO.Patient;
using MedGETA.Hospitals;
using MedGETA.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedGETA.Controllers
{
    public class PatientsHospitalController : ControllerBase
    {
        private readonly MedGETADbContext _context;

        public PatientsHospitalController(MedGETADbContext context)
        {
            _context = context;
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
    }
}
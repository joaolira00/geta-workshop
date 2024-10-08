using MedGETA.Context;
using MedGETA.DTO.Hospital;
using MedGETA.Hospitals;
using Microsoft.AspNetCore.Mvc;

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
    }
}
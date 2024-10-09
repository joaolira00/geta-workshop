using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MedGETA.Hospitals;
using MedGETA.Patients;

namespace MedGETA.Records
{
    public class Record
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        public string? PatientName { get; set; }

        [Required]
        public string? HospitalName { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid GenerateUUID()
        {
            Id = Guid.NewGuid();
            return Id;
        }

        //1:N with Patient
        [Required]
        public Guid PatientId { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }

        //1:N with Hospitals
        [Required]
        public Guid? HospitalId { get; set; }
        [JsonIgnore]
        public Hospital? Hospital { get; set; }
    }
}
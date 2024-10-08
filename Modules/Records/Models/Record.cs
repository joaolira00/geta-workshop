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
        public string? Diagnostic { get; set; }

        public Guid GenerateUUID()
        {
            Id = Guid.NewGuid();
            return Id;
        }

        //1:N with Patient
        public Guid PatientId { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }

        //1:N with Hospitals
        public Guid? HospitalId { get; set; }
        [JsonIgnore]
        public Hospital? Hospital { get; set; }
    }
}
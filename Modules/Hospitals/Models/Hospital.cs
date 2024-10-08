using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MedGETA.Patients;
using MedGETA.Records;

namespace MedGETA.Hospitals
{
    public class Hospital
    {   
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Address { get; set; }

        public Guid GenerateUUID()
        {
            Id = Guid.NewGuid();
            return Id;
        }

        //N:N with Patients
        [JsonIgnore]
        public ICollection<Patient>? Patients { get; set; }

        //1:N with Records
        [JsonIgnore]
        public ICollection<Record>? Records { get; set; }

    }
}
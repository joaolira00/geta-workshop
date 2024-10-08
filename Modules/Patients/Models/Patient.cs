using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MedGETA.Hospitals;
using MedGETA.Records;

namespace MedGETA.Patients
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Email { get; set; }

        //N:N with Hospitals
        [JsonIgnore]
        public ICollection<Hospital>? Hospitals { get; set; }

        //1:N with Record
        [JsonIgnore]
        public ICollection<Record>? Records { get; set; }

        public Patient()
        {

        }

        public Guid GenerateUUID()
        {
            Id = Guid.NewGuid();
            return Id;
        }
    }
}
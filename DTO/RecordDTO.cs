namespace MedGETA.DTO
{
    public class RecordDto
    {
        public string? Description { get; set; }
        public Guid PatientId { get; set; }
        public Guid HospitalId { get; set; }
    }
}

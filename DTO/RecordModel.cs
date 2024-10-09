namespace MedGETA.DTO.Record
{
    public class RecordModel
    {
        public Guid Id { get; set; }
        public string? PatientName { get; set; }
        public string? HospitalName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
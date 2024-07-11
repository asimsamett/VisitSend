namespace VisitSend.Models
{
    public class Visit
    {
        public short Id { get; set; }
        public short DoctorId { get; set; }
        public short PatientId { get; set; }
        public string Description { get; set; }
    }
}

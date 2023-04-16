namespace AppointmentScheduler.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        public string ManagerId { get; set; }
        public string AssociateId { get; set; }
        public bool IsManagerApproved { get; set; }
        public string? AdminId { get; set; }
    }
}

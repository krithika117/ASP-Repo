namespace AppointmentScheduler.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public int Duration { get; set; }
        public string? AssociateId { get; set; }
        public string? ManagerId { get; set; }
        public bool IsManagerApproved { get; set; }
        public string? AdminId { get; set; }
        public string? ManagerName { get; set; }
        public string? AssociateName { get; set; }
        public string? AdminName { get; set; }
        public bool IsForClient { get; set; }

    }
}

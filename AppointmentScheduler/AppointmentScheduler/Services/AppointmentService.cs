using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using System.Globalization;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<int> AddUpdate(AppointmentViewModel model)
        {
            var startDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture).AddMinutes(Convert.ToDouble(model.Duration));

            // Update logic
            if (model != null && model.Id > 0) {
                return 1;
            }
            
            // Create logic
            else {
                Console.WriteLine("Triggered create");
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    ManagerId = model.ManagerId,
                    AssociateId = model.AssociateId,
                    IsManagerApproved = false,
                    AdminId = model.AdminId
                };
                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }
        }

     

        public List<AssociateViewModel> GetAssociateList()
        {

            var associates = (from user in _db.Users
                            join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                            join roles in _db.Roles.Where(x => x.Name == Helper.Associate) on userRoles.RoleId equals roles.Id
                            select new AssociateViewModel
                            {
                                Id = user.Id,
                                Name = user.Name
                            }
                           ).ToList();

            return associates;
        }

        public List<ManagerViewModel> GetManagerList()
        {
            var managers = (from user in _db.Users
                            join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                            join roles in _db.Roles.Where(x => x.Name == Helper.Manager) on userRoles.RoleId equals roles.Id
                            select new ManagerViewModel
                            {
                                Id = user.Id,
                                Name = user.Name
                            }
                           ).ToList();

            return managers;
        }
        public List<AppointmentViewModel> AssociatesEventsById(string associateId)
        {
            return _db.Appointments.Where(x => x.AssociateId == associateId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("MM-dd-yyyy HH:mm:ss"),
                EndDate = c.EndDate?.ToString("MM-dd-yyyy HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsManagerApproved = c.IsManagerApproved,

            }).ToList();
        }        
        public List<AppointmentViewModel> ManagersEventsById(string managerId)
        {
            return _db.Appointments.Where(x => x.ManagerId == managerId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("MM-dd-yyyy HH:mm:ss"),
                EndDate = c.EndDate?.ToString("MM-dd-yyyy HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsManagerApproved = c.IsManagerApproved,

            }).ToList();

        }

}
}

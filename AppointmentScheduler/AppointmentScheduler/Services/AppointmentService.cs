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
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

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
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate?.ToString("yyyy-MM-dd HH:mm:ss"),
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
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsManagerApproved = c.IsManagerApproved,

            }).ToList();
        }

        public AppointmentViewModel GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsManagerApproved = c.IsManagerApproved,
                AssociateId = c.AssociateId,
                ManagerId = c.ManagerId,
                AssociateName = _db.Users.Where(x => x.Id == c.AssociateId).Select(x => x.Name).FirstOrDefault(),
                ManagerName = _db.Users.Where(x => x.Id == c.ManagerId).Select(x => x.Name).FirstOrDefault(),
            }).SingleOrDefault();
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);                
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsManagerApproved = true;
                return await _db.SaveChangesAsync();
            }
            return 0;
        }
    }
}

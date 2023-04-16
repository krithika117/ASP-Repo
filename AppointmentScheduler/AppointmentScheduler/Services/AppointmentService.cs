using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
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
    }
}

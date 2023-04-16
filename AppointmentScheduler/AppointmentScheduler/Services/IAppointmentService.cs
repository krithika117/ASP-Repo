using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppointmentService
    {
        public List<ManagerViewModel> GetManagerList();
        public List<AssociateViewModel> GetAssociateList();

    }
}

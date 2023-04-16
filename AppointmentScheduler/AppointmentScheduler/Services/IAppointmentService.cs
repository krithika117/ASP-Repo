using AppointmentScheduler.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AppointmentScheduler.Services
{
    public interface IAppointmentService
    {
        public List<ManagerViewModel> GetManagerList();
        public List<AssociateViewModel> GetAssociateList();
        public Task<int> AddUpdate(AppointmentViewModel model);
        public List<AppointmentViewModel> ManagersEventsById(string managerId);
        public List<AppointmentViewModel> AssociatesEventsById(string associateId);

        public AppointmentViewModel GetById(int id);

        public Task<int> Delete(int id);
        public Task<int> ConfirmEvent(int id);


    }
}

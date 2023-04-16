using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewBag.ManagerList = _appointmentService.GetManagerList();
            ViewBag.AssociateList = _appointmentService.GetAssociateList();
            ViewBag.Duration = Helper.GetTimeDropDown();
            return View();
        }
    }
}

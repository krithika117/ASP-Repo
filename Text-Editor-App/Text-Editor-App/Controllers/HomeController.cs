using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Text_Editor_App.Models;

namespace Text_Editor_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
   

        // GET: Home
        public IActionResult Index()
        {
            return View(new DocumentArea());
        }

        [HttpPost]
        public IActionResult Index(DocumentArea doc)
        {
            return View(doc);
        }
    

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
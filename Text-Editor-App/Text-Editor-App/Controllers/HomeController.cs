using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Text_Editor_App.Models;

namespace Text_Editor_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
   

        // GET: Home
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Editor()
        {
            return View(new DocumentArea());
        }

        [HttpPost]
        public IActionResult Editor(DocumentArea doc)
        {
            return View(doc);
        }

        public async Task<IActionResult> ViewNotes()
        {
            var notes = await _dbContext.Documents.ToListAsync();
            ViewBag.DocList = notes;
            return View(notes);
        }


        [HttpPost]
        public async Task<IActionResult> Save(string DocumentText, string Author)
        {

            var document = new DocumentArea
            {
                Author = Author,
                DocumentText = DocumentText
            };

            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        } 
        
        public IActionResult Edit(int Id)
        {
            var document = _dbContext.Documents.FirstOrDefault(x => x.Id == Id);
            return View(document);
        }
        
        public async Task<IActionResult> Delete(int Id)
        {
            var document = _dbContext.Documents.FirstOrDefault(x => x.Id == Id);
            if (document == null) return NotFound();
            _dbContext.Documents.Remove(document);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ViewNotes");
        }
        [HttpPost]
        public async Task<IActionResult> Update(int Id, string DocumentText, string Author)
        {
            var document = _dbContext.Documents.FirstOrDefault(x => x.Id == Id);

            if(document != null) { 
            
                document.Author = Author;
                document.DocumentText = DocumentText;
          
            _dbContext.Documents.Update(document);
            await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("ViewNotes");
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
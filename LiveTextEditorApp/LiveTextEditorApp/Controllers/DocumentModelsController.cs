using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiveTextEditorApp.Data;
using LiveTextEditorApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LiveTextEditorApp.Controllers
{
    public class DocumentModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;


        public DocumentModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocumentModels
       
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var notes = await _context.Documents
                .Where(n => n.UserId == user.Email)
                .ToListAsync();

            return View(notes);
        }


        // GET: DocumentModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var documentModel = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentModel == null)
            {
                return NotFound();
            }

            return View(documentModel);
        }

        // GET: DocumentModels/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocumentModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title,Content,UserId")] DocumentModel documentModel)
        {
            var user = await _userManager.GetUserAsync(User);
            documentModel.UserId = user.Email;

            _context.Add(documentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: DocumentModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine("GET Edit");
            if (id == null)
            {
                return NotFound();
            }

            var documentModel = await _context.Documents.FindAsync(id);
            if (documentModel == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (documentModel.UserId != user.Email)
            {
                return Forbid();
            }

            return View(documentModel);
        }

        // POST: DocumentModels/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId")] DocumentModel documentModel)
        {

            if (id != documentModel.Id)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            //documentModel.UserId = user.Email;
            Console.WriteLine(documentModel.Id);
            Console.WriteLine(documentModel.Content);
            Console.WriteLine(documentModel.Title);
            Console.WriteLine(documentModel.UserId);


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var err in errors)
                {
                    Console.WriteLine(err.ErrorMessage);
                }
            }
                
            if (ModelState.IsValid)
            {
                Console.WriteLine("IPOST Edit");
                try
                {
               
                    if (documentModel.UserId != user.Email)
                    {
                        return Forbid();
                    }

                    _context.Update(documentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentModelExists(documentModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(documentModel);
        }

        // GET: DocumentModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var documentModel = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentModel == null)
            {
                return NotFound();
            }

            return View(documentModel);
        }

        // POST: DocumentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Documents'  is null.");
            }
            var documentModel = await _context.Documents.FindAsync(id);
            if (documentModel != null)
            {
                _context.Documents.Remove(documentModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentModelExists(int id)
        {
          return (_context.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

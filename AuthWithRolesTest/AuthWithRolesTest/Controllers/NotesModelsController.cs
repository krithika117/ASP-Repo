﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthWithRolesTest.Data;
using AuthWithRolesTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AuthWithRolesTest.Controllers
{
    public class NotesModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
            
        private readonly UserManager<IdentityUser> _userManager;


        public NotesModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context; _userManager = userManager;

        }

        // GET: NotesModels
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var notes = await _context.NotesModel
                .Where(n => n.UserId == user.UserName)
                .ToListAsync();

            return View(notes);
        }

        // GET: NotesModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
          if (id == null)
            {
                return NotFound();
            }

            var notesModel = await _context.NotesModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notesModel == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (notesModel.UserId != user.Email)
            {
                return Forbid();
            }

            return View(notesModel);
        }
        [Authorize(Roles = "Manager, Member")]
        // GET: NotesModels/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Manager, Member")]
        // POST: NotesModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, Content, UserId")] NotesModel notesModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            notesModel.UserId = user.UserName;
            Console.WriteLine(notesModel.Title);
            Console.WriteLine(notesModel.Content);
            Console.WriteLine(notesModel.UserId);
            Console.WriteLine("Inner Hit");
            Console.WriteLine("HTTP Hit");
            _context.Add(notesModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            //return View(notesModel);
        }

        // GET: NotesModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notesModel = await _context.NotesModel.FindAsync(id);
            if (notesModel == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (notesModel.UserId != user.UserName)
            {
                return Forbid();
            }

            return View(notesModel);
        }

        // POST: NotesModels/Edit/5
        [Authorize(Roles = "Manager, Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId")] NotesModel notesModel)
        {
            if (id != notesModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    if (notesModel.UserId != user.Email)
                    {
                        return Forbid();
                    }

                    //notesModel.UserId = user.Email;

                    _context.Update(notesModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotesModelExists(notesModel.Id))
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
            return View(notesModel);
        }

        // GET: NotesModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotesModel == null)
            {
                return NotFound();
            }

            var notesModel = await _context.NotesModel
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notesModel == null)
            {
                return NotFound();
            }

            return View(notesModel);
        }

        // POST: NotesModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotesModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NotesModel'  is null.");
            }
            var notesModel = await _context.NotesModel.FindAsync(id);
            if (notesModel != null)
            {
                _context.NotesModel.Remove(notesModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotesModelExists(int id)
        {
          return (_context.NotesModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

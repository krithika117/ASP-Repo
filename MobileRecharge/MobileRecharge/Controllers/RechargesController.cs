using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileRecharge.Data;
using MobileRecharge.Models;

namespace MobileRecharge.Controllers
{
    [Authorize]
    public class RechargesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RechargesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        // GET: Recharges
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recharge.Include(r => r.Plan);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "User")]
        // GET: Recharges
        public async Task<IActionResult> MyRecharges()
        {
            var user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Recharge.Include(r => r.Plan).Where(r => r.UserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Recharges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recharge == null)
            {
                return NotFound();
            }

            var recharge = await _context.Recharge
                .Include(r => r.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recharge == null)
            {
                return NotFound();
            }

            return View(recharge);
        }

        // GET: Recharges/Create
        public IActionResult Create()
        {
            ViewData["PlanId"] = new SelectList(_context.MobilePlans, "Id", "Id");
            return View();
        }

        // POST: Recharges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PlanId")] Recharge recharge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recharge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.MobilePlans, "Id", "Id", recharge.PlanId);
            return View(recharge);
        }

        // GET: Recharges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recharge == null)
            {
                return NotFound();
            }

            var recharge = await _context.Recharge.FindAsync(id);
            if (recharge == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.MobilePlans, "Id", "Id", recharge.PlanId);
            return View(recharge);
        }

        // POST: Recharges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PlanId")] Recharge recharge)
        {
            if (id != recharge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recharge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RechargeExists(recharge.Id))
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
            ViewData["PlanId"] = new SelectList(_context.MobilePlans, "Id", "Id", recharge.PlanId);
            return View(recharge);
        }

        // GET: Recharges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recharge == null)
            {
                return NotFound();
            }

            var recharge = await _context.Recharge
                .Include(r => r.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recharge == null)
            {
                return NotFound();
            }

            return View(recharge);
        }

        // POST: Recharges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recharge == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recharge'  is null.");
            }
            var recharge = await _context.Recharge.FindAsync(id);
            if (recharge != null)
            {
                _context.Recharge.Remove(recharge);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RechargeExists(int id)
        {
          return (_context.Recharge?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

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
using MobileRecharge.Data.Migrations;
using MobileRecharge.Models;

namespace MobileRecharge.Controllers
{

    [Authorize]
    public class MobilePlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MobilePlansController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MobilePlans
        public async Task<IActionResult> Index()
        {
              return _context.MobilePlans != null ? 
                          View(await _context.MobilePlans.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MobilePlans'  is null.");
        }

        // POST: Create Payment
        [Authorize(Roles = "User")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayment([Bind("UserId, PlanId")] Recharge recharge)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            recharge.UserId = user.UserName;
            if (ModelState.IsValid)
            {

                _context.Add(recharge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recharge);
        }

        

        // GET: MobilePlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MobilePlans == null)
            {
                return NotFound();
            }

            var mobilePlan = await _context.MobilePlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mobilePlan == null)
            {
                return NotFound();
            }

            return View(mobilePlan);
        }

        // GET: MobilePlans/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MobilePlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceProvider,PlanName,Mode,NoOfMonths,Amount")] MobilePlan mobilePlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mobilePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mobilePlan);
        }

        [Authorize(Roles = "Admin")]
        // GET: MobilePlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MobilePlans == null)
            {
                return NotFound();
            }

            var mobilePlan = await _context.MobilePlans.FindAsync(id);
            if (mobilePlan == null)
            {
                return NotFound();
            }
            return View(mobilePlan);
        }

        // POST: MobilePlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceProvider,PlanName,Mode,NoOfMonths,Amount")] MobilePlan mobilePlan)
        {
            if (id != mobilePlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mobilePlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MobilePlanExists(mobilePlan.Id))
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
            return View(mobilePlan);
        }

        // GET: MobilePlans/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MobilePlans == null)
            {
                return NotFound();
            }

            var mobilePlan = await _context.MobilePlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mobilePlan == null)
            {
                return NotFound();
            }

            return View(mobilePlan);
        }

        // POST: MobilePlans/Delete/5
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MobilePlans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MobilePlans'  is null.");
            }
            var mobilePlan = await _context.MobilePlans.FindAsync(id);
            if (mobilePlan != null)
            {
                _context.MobilePlans.Remove(mobilePlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MobilePlanExists(int id)
        {
          return (_context.MobilePlans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

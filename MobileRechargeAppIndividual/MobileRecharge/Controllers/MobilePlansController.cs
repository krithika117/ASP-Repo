using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
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
        
        // GET: CustomerTransactions
        public async Task<IActionResult> CustomerTransactions()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var recharges = await _context.Recharge
                .Where(r => r.UserId == userId)
                .Join(_context.MobilePlans, r => r.PlanId, mp => mp.Id, (r, mp) => new { Recharge = r, MobilePlan = mp })
                .ToListAsync();

            ViewBag.customerPayments = recharges;
            return View();
        }

        // GET: AllCustomerTransactions
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllCustomerTransactions()
        {
            //var recharges = await _context.Recharge
            //    .Join(_context.MobilePlans, r => r.PlanId, mp => mp.Id, (r, mp) => new { Recharge = r, MobilePlan = mp })
            //    .ToListAsync();

            var recharges = await _context.Recharge
                            .Join(_context.MobilePlans, r => r.PlanId, mp => mp.Id, (r, mp) => new { Recharge = r, MobilePlan = mp })
                            .Join(_context.Users, rp => rp.Recharge.UserId, u => u.Id, (rp, u) => new { Recharge = rp.Recharge, MobilePlan = rp.MobilePlan, UserName = u.UserName })
                            .ToListAsync();

            ViewBag.allCustomerPayments = recharges;
            return View();
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var plan = await _context.MobilePlans.FindAsync(id);

            var recharge = new Recharge
            {
                PlanId = plan.Id,
                UserId = user.Id
            };
            _context.Recharge.Add(recharge);
            await _context.SaveChangesAsync();

            
            return RedirectToAction("CustomerTransactions");

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
        public IActionResult SendMail()
        {
            return View(new EnquiryMail());
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(EnquiryMail model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(user.Email, user.Email));
                message.To.Add(new MailboxAddress("Admin", "promote.n0replymailer@gmail.com"));
                message.Subject = model.Subject;
                message.Body = new TextPart("plain")
                {
                    Text = model.Body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("promote.n0replymailer@gmail.com", "ashqtcouelkfiznr");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                ViewBag.Message = "Email sent!";
            }

            return View(model);
        }
        private bool MobilePlanExists(int id)
        {
          return (_context.MobilePlans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

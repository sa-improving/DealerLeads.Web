using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DealerLead.Web.Controllers
{
    public class DealershipsController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public DealershipsController(DealerLeadDbContext context)
        {
            _context = context;
        }

        private int UserId(ClaimsPrincipal principal)
        {
            Guid OID = (Guid)IdentityHelper.GetAzureOIDToken(principal);
            return _context.DealerLeadUser.FirstOrDefault(x => x.OID == OID).Id;
        }

        //private int UserId(ClaimsPrincipal principal)
        //{
        //    var userId = HttpContext.Session.Get<int>("UserId");
        //    if(userId == 0)
        //    {
        //        Guid OID = (Guid)IdentityHelper.GetAzureOIDToken(principal);
        //        userId = _context.DealerLeadUser.FirstOrDefault(x => x.OID == OID).Id;
        //    }
        //    HttpContext.Session.Set("UserId", userId);
        //    return userId;
        //}

        //GET: Dealerships for logged in user
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            int userId = UserId(principal);
            return View(await _context.Dealership.Where(d => d.UserId == userId).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealership.FirstOrDefaultAsync(d => d.Id == id);
            if (dealership == null)
            {
                return NotFound();
            }
            return View(dealership);
        }

        public IActionResult Create()
        {
            ViewBag.State = new SelectList(_context.SupportedState, "Abbreiviation", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address1,Address2,City,State,Zipcode,UserId")] Dealership dealership)
        {
            if(ModelState.IsValid)
            {
                ClaimsPrincipal principal = User as ClaimsPrincipal;
                dealership.UserId = UserId(principal);
                _context.Add(dealership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealership.FindAsync(id);
            if (dealership == null)
            {
                return NotFound();
            }
            ViewBag.State = new SelectList(_context.SupportedState, "Abbreiviation", "Name", dealership.State);
            return View(dealership);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address1,Address2,City,State,Zipcode,UserId")] Dealership dealership)
        {
            if(id != dealership.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    ClaimsPrincipal principal = User as ClaimsPrincipal;
                    dealership.UserId = UserId(principal);
                    dealership.ModifyDate = DateTime.Now;
                    _context.Update(dealership);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!DealershipExists(dealership.Id))
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
            ViewBag.State = new SelectList(_context.SupportedState, "Abbreiviation", "Name", dealership.State);
            return View(dealership);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealership.FirstOrDefaultAsync(d => d.Id == id);
            if(dealership == null)
            {
                return NotFound();
            }

            return View(dealership);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dealership = await _context.Dealership.FindAsync(id);
            _context.Dealership.Remove(dealership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool DealershipExists(int id)
        {
            return _context.Dealership.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealerLead;
using System.Security.Claims;

namespace DealerLead.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public VehiclesController(DealerLeadDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            int userId = IdentityHelper.UserId(principal, _context);
            List<Dealership> dealerships = _context.Dealership.Where(d => d.UserId == userId).ToList();
            List<int> dealershipIds = new List<int>();
            dealerships.ForEach(d => dealershipIds.Add(d.Id));
            var dealerLeadDbContext = _context.Vehicle.Include(v => v.Dealership).Include(v => v.Model).Where(v => dealershipIds.Contains(v.DealershipId));
            return View(await dealerLeadDbContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Dealership)
                .Include(v => v.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            int userId = IdentityHelper.UserId(principal, _context);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.UserId == userId), "Id", "Name");
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelId,MSRP,StockNumber,Color,DealershipId,SellDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            int userId = IdentityHelper.UserId(principal, _context);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.UserId == userId), "Id", "Name", vehicle.DealershipId);
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            int userId = IdentityHelper.UserId(principal, _context);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.UserId == userId), "Id", "Name", vehicle.DealershipId);
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelId,MSRP,StockNumber,Color,DealershipId,SellDate")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            int userId = IdentityHelper.UserId(principal, _context);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.UserId == userId), "Id", "Name", vehicle.DealershipId);
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Dealership)
                .Include(v => v.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}

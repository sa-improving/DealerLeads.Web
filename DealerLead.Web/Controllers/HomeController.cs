using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DealerLead.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DealerLead.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DealerLeadDbContext _context;

        public HomeController(ILogger<HomeController> logger, DealerLeadDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal principal = User as ClaimsPrincipal;
                Guid Oid = (Guid)IdentityHelper.GetAzureOIDToken(principal);
                var dealerUser = _context.DealerLeadUser.FirstOrDefault(u => u.OID == Oid);
                if(dealerUser == null)
                {
                    ViewBag.IsRegistered = false;
                }
                else
                {
                    ViewBag.IsRegistered = true;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id", "OID")] DealerLeadUser dealerLeadUser)
        {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            Guid OiD = (Guid)IdentityHelper.GetAzureOIDToken(principal);
            dealerLeadUser.OID = OiD;
            _context.Add(dealerLeadUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public static class IdentityHelper
        {
            public static Guid? GetAzureOIDToken(ClaimsPrincipal claimsPrincipal)
            {
                if (claimsPrincipal == null)
                {
                    return null;
                }
                if (claimsPrincipal.Identity.IsAuthenticated == false)
                {
                    return null;
                }
                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
                if (claimsIdentity == null)
                {
                    return null;
                }
                var oidClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
                if (oidClaim == null)
                {
                    return null;
                }
                return Guid.Parse(oidClaim.Value);
            }
        }
        
        


        [Authorize]
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
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

        public static int UserId(ClaimsPrincipal principal, DealerLeadDbContext context)
        {
            Guid OID = (Guid)IdentityHelper.GetAzureOIDToken(principal);
            return context.DealerLeadUser.FirstOrDefault(x => x.OID == OID).Id;
        }
    }
}

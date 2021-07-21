using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DealerLead
{
    public static class Authentication
    {
        private static DealerLeadDbContext _dbContext;

        static Authentication()
        {
            _dbContext = new DealerLeadDbContext();
        }

        public static async Task OnTokenValidatedFunc(TokenValidatedContext context)
        {
            // Custom code here
            Guid? azureOIDToken = IdentityHelper.GetAzureOIDToken(context.Principal);
            var user = _dbContext.DealerLeadUser.FirstOrDefault(x => x.OID == azureOIDToken);

            if (user == null)
            {
                user = new DealerLeadUser { OID = azureOIDToken.Value };
                _dbContext.Add(user);
                _dbContext.SaveChanges();
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}

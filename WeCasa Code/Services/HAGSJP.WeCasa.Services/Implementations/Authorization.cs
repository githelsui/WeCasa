using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using Microsoft.Identity.Client;

namespace HAGSJP.WeCasa.Services.Implementations
{
    internal class Authorization : IAuthorization
    {
        private readonly IAuthorizationDAO _dao;

        public Authorization(IAuthorizationDAO dao)
        {
            _dao = dao;
        }

        public bool ValidateAdminRole(UserAccount ua)
        {
            var userRole = _dao.GetRole(ua);
            if(userRole == Models.Security.UserRoles.AdminUser)
            {
                return true;
            }
            return false;
        }

        public bool ValidateClaim(UserAccount ua, Claim targetClaim)
        {
            List<Claim> userClaims = _dao.GetClaims(ua).UserClaims;
            foreach (Claim claim in userClaims)
            {
                if(targetClaim.ClaimType == claim.ClaimType && targetClaim.ClaimValue == claim.ClaimValue)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

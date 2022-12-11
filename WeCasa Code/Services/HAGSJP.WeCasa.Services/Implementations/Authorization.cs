using System;
using System.Collections.Generic;
using System.Linq;
using HAGSJP.WeCasa.Logging.Implementations;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using Microsoft.Identity.Client;
using HAGSJP.WeCasa.sqlDataAccess;
using Logger = HAGSJP.WeCasa.Logging.Implementations.Logger;
using LogLevel = HAGSJP.WeCasa.Models.LogLevel;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class AuthorizationService : IAuthorization
    {
        private readonly IAuthorizationDAO _dao;
        private Logger successlogger;

        public AuthorizationService(IAuthorizationDAO dao)
        {
            _dao = dao;
            successlogger = new Logger(new AccountMariaDAO());
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

        public ResultObj ValidateClaim(UserAccount ua, Claim targetClaim)
        {
            var result = new ResultObj();

            List<Claim> userClaims = _dao.GetClaims(ua).UserClaims;
            foreach (Claim claim in userClaims)
            {
                if(targetClaim.ClaimType == claim.ClaimType && targetClaim.ClaimValue == claim.ClaimValue)
                {
                    result.ReturnedObject = true;
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    return result;
                }
            }

            // Unauthorized User Scenarios
            var successUnauthLogMsg = "Unauthorized user denied access." + targetClaim.ClaimType + " access.";
            successlogger.Log(successUnauthLogMsg, LogLevel.Info, "Data Store", ua.Username);
            result.ReturnedObject = false;
            result.IsSuccessful = true;
            if (targetClaim.ClaimType.Equals("Functionality"))
            {
                result.Message = "Unauthorized access to functionality.";
            }
            if (targetClaim.ClaimType.Equals("Read"))
            {
                result.Message = "Unauthorized access to read data.";
            }
            if (targetClaim.ClaimType.Equals("Write"))
            {
                result.Message = "Unauthorized access to modify data.";
            }
            if (targetClaim.ClaimType.Equals("View"))
            {
                result.Message = "Unauthorized access to view.";
            }
            return result;
        }

    }
}

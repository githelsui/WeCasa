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
using HAGSJP.WeCasa.Models.Security;
using System.Collections;

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

        public ResultObj ValidateAdminRole(UserAccount ua)
        {
            var result = new ResultObj();

            // Preconditions:
            // Check if user is active/logged in
            ResultObj daoResultActivity = _dao.GetActiveStatus(ua);
            if (daoResultActivity == null)
            {
                // Failure case from previous layer
                return daoResultActivity;
            }
            bool isActiveSession = (bool)daoResultActivity.ReturnedObject;
            if(isActiveSession == false)
            {
                result.IsSuccessful = false;
                result.Message = "User is not logged in. Cannot validate admin role.";
                return result;
            }

            ResultObj daoResultClaims = _dao.GetRole(ua);
            if(daoResultClaims == null)
            {
                // Failure case from previous layer
                return daoResultClaims;
            }
            var userRole = daoResultClaims.ReturnedObject;
            UserRoles roleEnum = (UserRoles)Enum.Parse(typeof(UserRoles), userRole.ToString());

            result.IsSuccessful = true;
            result.Message = string.Empty;
            if (roleEnum == Models.Security.UserRoles.AdminUser)
            {
                result.ReturnedObject = true;
            } else
            {
                result.ReturnedObject = false;
            }
            return result;
        }

        public ResultObj ValidateClaim(UserAccount ua, Claim targetClaim)
        {
            var result = new ResultObj();

            //Preconditions
            // Check if user is active/logged in
            ResultObj daoResultActivity = _dao.GetActiveStatus(ua);
            if (daoResultActivity == null)
            {
                // Failure case from previous layer
                return daoResultActivity;
            }
            bool isActiveSession = (bool)daoResultActivity.ReturnedObject;
            if (isActiveSession == false)
            {
                result.IsSuccessful = false;
                result.Message = "User is not logged in. Cannot authorize access.";
                return result;
            }

            ResultObj daoResult = _dao.GetClaims(ua);
            if (daoResult == null)
            {
                // Failure case from previous layer
                return daoResult;
            }

            Claims claims = (Claims)daoResult.ReturnedObject;
            List<Claim> userClaims = (List<Claim>)claims.UserClaims;
            foreach (Claim claim in userClaims)
            {
                if(targetClaim.ClaimType == claim.ClaimType && targetClaim.ClaimValue == claim.ClaimValue)
                {
                    result.ReturnedObject = true;
                    result.IsSuccessful = true;
                    result.Message = $"Authorized access to {targetClaim.ClaimType} Permissions.";
                    return result;
                }
            }

            // Unauthorized User Scenarios
            var successUnauthLogMsg = $"Unauthorized access to {targetClaim.ClaimType} Permissions.";
            successlogger.Log(successUnauthLogMsg, LogLevel.Info, "Data Store", ua.Username);
            result.ReturnedObject = false;
            result.IsSuccessful = true;
            result.Message = successUnauthLogMsg;
            return result;
        }

    }
}

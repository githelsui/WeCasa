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
using LogLevels = HAGSJP.WeCasa.Models.LogLevels;
using HAGSJP.WeCasa.Models.Security;
using System.Collections;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class AuthorizationService : IAuthorization
    {
        private readonly IAuthorizationDAO _dao;
        private Logger successLogger;
        private Logger errorLogger;

        public AuthorizationService(IAuthorizationDAO dao)
        {
            _dao = dao;
            successLogger = new Logger(new AccountMariaDAO());
            errorLogger = new Logger(new AccountMariaDAO());
        }

        public AuthResult ValidateAdminRole(UserAccount ua)
        {
            var result = new AuthResult();

            // Preconditions:
            // Check if user is active/logged in
            AuthResult daoResultActivity = ValidateActiveUser(ua);
            if (daoResultActivity.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultActivity.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultActivity;
            }
            bool isActive = (bool)daoResultActivity.ReturnedObject;
            if (isActive == false)
            {
                result.Message = "Successfully denied access to unauthorized, logged out user.";
                successLogger.Log(result.Message, LogLevels.Info, "Data Store", ua.Username);
                result.IsSuccessful = true;
                return result;
            }

            AuthResult daoResultRoles = _dao.GetRole(ua);
            if (daoResultRoles.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultRoles.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultRoles;
            }
            var userRoleObj = daoResultRoles.ReturnedObject;
            UserRoles userRole = (UserRoles)Enum.Parse(typeof(UserRoles), userRoleObj.ToString());

            result.IsSuccessful = true;
            result.Message = string.Empty;
            if (userRole == UserRoles.AdminUser)
            {
                result.ReturnedObject = true;
            }
            else
            {
                result.ReturnedObject = false;
            }
            return result;
        }

        public AuthResult ValidateClaim(UserAccount ua, Claim targetClaim)
        {
            var result = new AuthResult();

            //Preconditions
            // Check if user is active/logged in
            AuthResult daoResultActivity = ValidateActiveUser(ua);
            if (daoResultActivity.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultActivity.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultActivity;
            }
            bool isActive = (bool)daoResultActivity.ReturnedObject;
            if (isActive == false)
            {
                result.Message = "Successfully denied to unauthorized, logged out user.";
                successLogger.Log(result.Message, LogLevels.Info, "Data Store", ua.Username);
                result.IsSuccessful = true;
                return result;
            }

            AuthResult daoResultClaims = _dao.GetClaims(ua);
            if (daoResultClaims.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultClaims.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultClaims;
            }

            Claims claims = (Claims)daoResultClaims.ReturnedObject;
            List<Claim> userClaims = (List<Claim>)claims.UserClaims;
            foreach (Claim claim in userClaims)
            {
                if (targetClaim.ClaimType.Equals(claim.ClaimType) && targetClaim.ClaimValue.Equals(claim.ClaimValue))
                {
                    result.ReturnedObject = true;
                    result.IsSuccessful = true;
                    result.Message = $"Authorized access to {targetClaim.ClaimType} Permissions.";
                    return result;
                }
            }

            // Unauthorized User Scenarios (Missing Permissions)
            var successUnauthLogMsg = $"Unauthorized access to {targetClaim.ClaimType} Permissions.";
            successLogger.Log(successUnauthLogMsg, LogLevels.Info, "Business", ua.Username);
            result.ReturnedObject = false;
            result.IsSuccessful = true;
            result.Message = successUnauthLogMsg;
            return result;
        }

        public AuthResult ValidateActiveUser(UserAccount ua)
        {
            var result = new AuthResult();

            //Preconditions: Check if user is active/logged in
            AuthResult daoResultActivity = _dao.GetActiveStatus(ua);
            if (daoResultActivity.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultActivity.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultActivity;
            }
            bool isActive = (bool)daoResultActivity.ReturnedObject;
            if (isActive == true)
            {
                result.Message = string.Empty;
                result.ReturnedObject = true;
                result.IsSuccessful = true;
                return result;
            }

            // Unauthorized User Scenario (Not enabled / Logged in)
            result.Message = "Successfully denied access to unauthorized, logged out user.";
            successLogger.Log(result.Message, LogLevels.Info, "Data Store", ua.Username);
            result.IsSuccessful = true;
            result.ReturnedObject = false;
            return result;
        }

        public AuthResult AddClaims(UserAccount ua, Claim newClaim)
        {
            AuthResult daoResultClaims = _dao.GetClaims(ua);
            if (daoResultClaims.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultClaims.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultClaims;
            }

            Claims claims = (Claims)daoResultClaims.ReturnedObject;
            List<Claim> userClaims = claims.UserClaims;
            userClaims.Add(newClaim);

            AuthResult daoResultInsertClaims = _dao.InsertClaims(ua, userClaims);
            if (daoResultInsertClaims.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultClaims.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultClaims;
            }

            // Successful Update of Claims
            return daoResultInsertClaims;
        }

        public AuthResult AddClaims(UserAccount ua, List<Claim> newClaims)
        {
            AuthResult daoResultClaims = _dao.GetClaims(ua);
            if (daoResultClaims.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultClaims.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultClaims;
            }

            Claims claims = (Claims)daoResultClaims.ReturnedObject;
            List<Claim> userClaims = claims.UserClaims;
            userClaims.AddRange(newClaims);

            AuthResult daoResultInsertClaims = _dao.InsertClaims(ua, userClaims);
            if (daoResultInsertClaims.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(daoResultClaims.Message, LogLevels.Error, "Data Store", ua.Username);
                return daoResultClaims;
            }

            // Successful Update of Claims
            return daoResultInsertClaims;
        }
    }
}

using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess; 
using System.Net;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class Authentication : IAuthentication
    {
        private readonly AccountMariaDAO _dao;
        private Logger successLogger;
        private Logger errorLogger;
        public Authentication()
        {
            _dao = new AccountMariaDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public Authentication(AccountMariaDAO dao)
        {
            _dao = dao;
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }
        public Result VerifyOTPassword(string email, string code)
        {
            var result = new Result();
            var checkValidChar = new Regex(@"^[a-zA-Z0-9.@\- ]*$");
            var checkLength = new Regex(@".{8,10}");
            if (!checkValidChar.IsMatch(code) || !checkLength.IsMatch(code))
            {
                result.IsSuccessful = false;
                result.Message = "Invalid username or password provided. Retry again or contact system administrator if issue persists.";
                return result;
            }

            else
            {
                result.IsSuccessful = true;
                return result;
            }
        }

        // Resets OTP code and time for a user account
        public Result ResetOTP(UserAccount userAccount)
        {
            var result = _dao.ResetOTP(userAccount);
            if(!result.IsSuccessful)
            {
                errorLogger.Log("Error updating one-time code", LogLevels.Error, "Data Store", userAccount.Username);
            }
            return result;
        }

        // Checks if the user already exists in the database and makes sure they are not authenticated or disabled
        public Result IsExistingAccount(UserAccount userAccount)
        {
            var result = _dao.GetUserInfo(userAccount);
            // User account checks
            // User is already authenticated
            if (result.IsAuth == true)
            {
                result.IsSuccessful = false;
                result.Message = "Account is already authenticated.";
            }
            // Invalid credentials
            else if (result.HasValidCredentials == false)
            {
                result.IsSuccessful = false;
                result.Message = "Invalid username or password provided. Retry again or contact the System Administrator.";
            }
            // Account is disabled
            else if (result.IsEnabled == false)
            {
                result.IsSuccessful = false;
                result.Message = "Account Disabled. Perform account recovery or contact the System Administrator.";
            }
            else
            {
                result.IsSuccessful = true;
                result.Message = "";
            }
            return result;
        }

        public Result AuthenticateUser(UserAccount userAccount, OTP otp)
        {
            var loginUser = _dao.AuthenticateUser(userAccount, otp);

            if (loginUser.HasValidOTP && loginUser.IsSuccessful && IsAccountEnabled(userAccount))
            {
                // Reset all authentication attempts upon successful login
                ResetAuthenticationAttempts(userAccount);
                ResetOTP(userAccount);
                // Logging the registration
                UserOperation loginSuccess = new UserOperation(Operations.Login, 1);
                successLogger.Log("Login successful", LogLevels.Info, "Data Store", userAccount.Username, loginSuccess);
            }
            else
            {
                if(!loginUser.HasValidOTP)
                {
                    loginUser.Message = "One-time code is invalid.";
                }
                if(loginUser.ExpiredOTP)
                {
                    loginUser.Message = "One-time code is expired.";
                }
                // Logging the error with ip address
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                UserOperation loginFailure = new UserOperation(Operations.Login, 0);
                errorLogger.Log("Error during Authentication from " + ip.AddressList[0].ToString(), LogLevels.Error, "Data Store", userAccount.Username, loginFailure);
            }
            return loginUser;
        }

        // Checks if there were 3 failed login attempts in the past 24 hours
        // On the third attempt, disable user account
        public Boolean IsAccountEnabled(UserAccount userAccount){
            List<DateTime> failedAttemptTimes = _dao.GetUserOperations(userAccount, Operations.Login);
            /*DateTime now = DateTime.Now;
            DateTime timeLimit = now.AddHours(-24);
            List<DateTime> itemsAfter = failedAttemptTimes.FindAll(i => i > timeLimit);*/

            if (failedAttemptTimes.Count >= 3) 
            {
                // Disabling account
                UserManager um = new UserManager();
                var result = um.DisableUser(userAccount);
                if (result.IsSuccessful)
                {
                    successLogger.Log("User has been disabled after 3 failed login attempts in the past 24 hours", LogLevels.Debug, "Business", userAccount.Username);
                }
                else
                {
                    errorLogger.Log("Error disabling user account.", LogLevels.Error, "Data Store", userAccount.Username);
                }
                return false;
            }
            else 
            {
                return true;
            }
        }
        // clears all failed attempts when the user successfully logs in OR when 
        // system recovery is performed
        public Result ResetAuthenticationAttempts(UserAccount userAccount) {
            var resetResult = new Result();
            resetResult = _dao.ResetAuthenticationAttempts(userAccount, Operations.Login);
            
            if (resetResult.IsSuccessful)
            {
                // Logging the successful reset
                successLogger.Log("Authentication attempts reset successful", LogLevels.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error 
                errorLogger.Log("Error while resetting authentication attempts", LogLevels.Error, "Data Store", userAccount.Username);

            }
            return resetResult;
        }


    }
}
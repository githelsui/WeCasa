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
                // Ensuring that the one-time code has not expired
                result.IsSuccessful = true;
                return result;
            }
        }
        public Result AuthenticateUser(string password, OTP otp)
        {
            var loginUserResult = new Result();
            UserAccount userAccount = new UserAccount(otp.Username);

            loginUserResult = _dao.AuthenticateUser(userAccount, password, otp);

            if (loginUserResult.IsSuccessful && IsAccountEnabled(userAccount))
            {
                // Reset all authentication attempts upon successful login
                ResetAuthenticationAttempts(userAccount);
                // Logging the registration
                successLogger.Log("Login successful", LogLevel.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error with ip address
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                errorLogger.Log("Error during Authentication from " + ip.AddressList[0].ToString(), LogLevel.Error, "Data Store", userAccount.Username);

            }
            return loginUserResult;
        }

        // Checks if there were 3 failed login attempts in the past 24 hours
        // On the third attempt, disable user account
        public Boolean IsAccountEnabled(UserAccount userAccount){
            List<DateTime> failedAttemptTimes = _dao.GetAuthenticationAttempts(userAccount);
            DateTime now = DateTime.Now;
            DateTime timeLimit = now.AddHours(-24);
            List<DateTime> itemsAfter = failedAttemptTimes.FindAll(i => i > timeLimit);
            Console.WriteLine("itemsAfter" + itemsAfter.Count);

            if (itemsAfter.Count >= 3) 
            {
                // disabling account
                UserManager um = new UserManager();
                um.DisableUser(userAccount);
                errorLogger.Log("User has been disabled after 3 failed login attempts in the past 24 hours", LogLevel.Debug, "Business", userAccount.Username);
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
            resetResult = _dao.ResetAuthenticationAttempts(userAccount);
            
            if (resetResult.IsSuccessful)
            {
                // Logging the successful reset
                successLogger.Log("Authentication attempts reset successful", LogLevel.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error 
                errorLogger.Log("Error while resetting authentication attempts", LogLevel.Error, "Data Store", userAccount.Username);

            }
            return resetResult;
        }


    }
}
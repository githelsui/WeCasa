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

            if (loginUserResult.IsSuccessful)
            {
                // Logging the registration
                successLogger.Log("Login successful", LogLevel.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error occurred during login attempt", LogLevel.Error, "Data Store", userAccount.Username);
            }
            return loginUserResult;
        }

    }
}

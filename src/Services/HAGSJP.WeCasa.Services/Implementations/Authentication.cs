using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess; 
using System.Net;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

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

        public AuthResult VerifyEncryptedPasswords(UserAccount userAccount)
        {
            var userInfoResult = _dao.ValidateUserInfo(userAccount);
            // User account checks
            // User is already authenticated
            if (userInfoResult.IsAuth == true)
            {
                userInfoResult.IsSuccessful = false;
                userInfoResult.Message = "Account is already authenticated.";
            }
            // Account is disabled
            else if (userInfoResult.IsEnabled == false)
            {
                userInfoResult.IsSuccessful = false;
                userInfoResult.Message = "Account Disabled. Perform account recovery or contact the System Administrator.";
            }
            // Invalid Email
            else if(userInfoResult.ExistingAcc == false)
            {
                userInfoResult.IsSuccessful = false;
                userInfoResult.Message = "Invalid email provided. Retry again or contact the System Administrator.";
            }
            else
            {
                HashSaltSecurity hashSaltSecurity = new HashSaltSecurity();
                userAccount.Salt = userInfoResult.Salt;
                userInfoResult = hashSaltSecurity.ValidateHashedPasswords(userAccount, userInfoResult);
            }
            return userInfoResult;
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

        public Result UpdateUserAuthentication(UserAccount userAccount, bool is_auth)
        {
            var result = _dao.UpdateUserAuthentication(userAccount, is_auth);
            if (!result.IsSuccessful)
            {
                errorLogger.Log($"Error updating user authentication status to {is_auth}", LogLevels.Error, "Data Store", userAccount.Username);
            }
            return result;
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

        // Used in front-end views
        public Result AuthenticateUser(UserAccount userAccount, OTP submittedOTP)
        {
            var authRes = _dao.ValidateUserInfo(userAccount);
            if (authRes.ExistingAcc)
            {
                OTP fetchedOTP = new OTP(userAccount.Username, authRes.OTPCode);
                return AuthenticateUser(userAccount, submittedOTP, fetchedOTP);
            }
            return authRes;
        }

        public Result AuthenticateUser(UserAccount userAccount, OTP userOtp, OTP otp)
        {
            var loginUser = new AuthResult();
            if (userOtp.Code == otp.Code)
            {
                // Checking expiration of otp
                loginUser = _dao.AuthenticateUser(userAccount, otp);
                if (loginUser.HasValidOTP && loginUser.IsSuccessful && IsAccountEnabled(userAccount))
                {
                    // Reset all authentication attempts upon successful login
                    var authAttemptsResult = ResetAuthenticationAttempts(userAccount);
                    // Changes OTP in data store to null
                    var otpResetResult = ResetOTP(userAccount);
                    // Updates authentication status of user
                    var authUpdateResult = UpdateUserAuthentication(userAccount, true);

                    if (authAttemptsResult.IsSuccessful && otpResetResult.IsSuccessful && authUpdateResult.IsSuccessful)
                    {
                        // Logs the successful login
                        UserOperation loginSuccess = new UserOperation(Operations.Login, 1);
                        successLogger.Log("Login successful", LogLevels.Info, "Data Store", userAccount.Username, loginSuccess);
                        loginUser.Message = "Successfully logged user in.";
                        return loginUser;
                    }
                    if (loginUser.ExpiredOTP)
                    {
                        loginUser.Message = "One-time code is expired.";
                    }
                }
            }
            else
            {
                loginUser.HasValidOTP = false;
                loginUser.Message = "One-time code is invalid.";
            }
            // Logging the error with ip address
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            UserOperation loginFailure = new UserOperation(Operations.Login, 0);
            errorLogger.Log("Error during Authentication from " + ip.AddressList[0].ToString(), LogLevels.Error, "Data Store", userAccount.Username, loginFailure);
            return loginUser;
        }

        // Checks if there were 3 failed login attempts in the past 24 hours
        // On the third attempt, disable user account
        public Boolean IsAccountEnabled(UserAccount userAccount){
            UserOperation userOperation = new UserOperation(Operations.Login, 0);
            List<DateTime> failedAttemptTimes = _dao.GetUserOperations(userAccount, userOperation);
            
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
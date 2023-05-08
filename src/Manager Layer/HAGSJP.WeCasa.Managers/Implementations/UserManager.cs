using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly AccountMariaDAO _dao;
        private readonly AuthorizationService _authService;
        private Logger successLogger;
        private Logger errorLogger;

        public UserManager()
        {
            _dao = new AccountMariaDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
            _authService = new AuthorizationService(new AuthorizationDAO());
        }
        public UserManager(AccountMariaDAO dao)
        {
            _dao = dao;
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }

        public Result ValidateName(string name)
        {
            var validName = new Result();
            var checkValidName = new Regex(@"\b([A-ZÀ-ÿ][-,a-z. ']*)+");
            validName.IsSuccessful = checkValidName.IsMatch(name);
            return validName;
        }

        // checks whether a new email has the correct characters
        public Result ValidateEmail(string email)
        {
            var validEmail = new Result();
            var checkValidChar = new Regex(@"^[a-zA-Z0-9.,@!\- ]*$");
            if (!MailAddress.TryCreate(email, out var mailAddress))
            {
                validEmail.IsSuccessful = false;
                validEmail.Message = "Invalid email provided. Retry again or contact the System Administrator.";
            }
            else if (email.Length > 255)
            {
                validEmail.IsSuccessful = false;
                validEmail.Message = "Invalid email provided. Retry again or contact the System Administrator.";

            }
            else if (!checkValidChar.IsMatch(email))
            {
                validEmail.IsSuccessful = false;
                validEmail.Message = "Invalid email provided. Retry again or contact the System Administrator.";

            }
            else
            {
                validEmail.IsSuccessful = true;
            }
            return validEmail;
        }

        public bool IsUsernameTaken(string username)
        {
            UserAccount userAccount = new UserAccount(username);
            var result = _dao.ValidateUserInfo(userAccount);
            return result.ExistingAcc;
        }
        public Result ValidatePassword(string password)
        {
            var result = new Result();
            var checkValidChar = new Regex(@"^[a-zA-Z0-9.,@!\- ]*$");
            var checkNumber = new Regex(@"[0-9]+");
            var checkUppercase = new Regex(@"[A-Z]+");
            var checkLowercase = new Regex(@"[a-z]+");
            var checkLength = new Regex(@".{8,80}");
            var checkSpecialChar = new Regex(@"[!@.,-]");

            if (checkValidChar.IsMatch(password) && checkLength.IsMatch(password) && checkNumber.IsMatch(password) && checkUppercase.IsMatch(password) && checkLowercase.IsMatch(password) && checkSpecialChar.IsMatch(password))
            {
                result.IsSuccessful = true;
                return result;
            }

            else
            {
                if (!checkLength.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Password: Password is not within the character range (8-80)";
                }
                else if (!checkUppercase.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Password: Password does not contain upper case letter";
                }
                else if (!checkLowercase.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Password: Password does not contain lower case letter";
                }
                else if (!checkNumber.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Password: Password does not contain a numeric value";
                }
                else if (!checkSpecialChar.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Password: Password does not contain a special case character";
                }
                else if (!checkValidChar.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Password: Password contains invalid characters";
                }
                else
                {
                    result.IsSuccessful = true;
                    result.Message = "";
                }
                return result;
            }
        }

        public OTP GenerateOTPassword(UserAccount userAccount)
        {
            var savingOTPresult = new Result();
            Random r = new Random();
            string code = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-@";
            foreach (var i in Enumerable.Range(0, 10))
            {
                code += chars[r.Next(0, 65)];
            }
            var otp = new OTP(userAccount.Username, code);

            // Saving activation code
            AccountMariaDAO dao = new AccountMariaDAO();
            savingOTPresult = dao.SaveCode(userAccount, otp);

            if (savingOTPresult.IsSuccessful)
            {
                // Logging the OTP save
                successLogger.Log("One-time password generated successfully", LogLevels.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error saving a one-time password", LogLevels.Error, "Data Store", userAccount.Username);
            }
            return otp;
        }

        public string ConfirmPassword(string password)
        {
            Console.WriteLine("Re-enter Password:");
            string confirmPassword = Console.ReadLine();
            if (confirmPassword == password)
            {
                return confirmPassword;
            }
            else
            {
                bool validC = false;
                while (validC == false)
                {
                    Console.WriteLine("Passwords do not match. Re-enter Password: ");
                    confirmPassword = Console.ReadLine();
                    if (confirmPassword == password)
                    {
                        validC = true;
                    }
                }
            }
            return confirmPassword;
        }

        public UserStatus PopulateUserStatus(UserAccount userAccount)
        {
            AuthResult populateResult = _dao.PopulateUserStatus(userAccount);
            return (UserStatus) populateResult.ReturnedObject;
        }

        public async Task<AuthResult> GetUserProfile(UserAccount userAccount)
        {
            var result = new AuthResult();
            try
            {
                var daoResult = await _dao.GetUserProfile(userAccount);
                if (!daoResult.IsSuccessful)
                {
                    await errorLogger.Log("User profile fetched failed from Users", LogLevels.Info, "Data Store", userAccount.Username);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                result.ReturnedObject = (UserProfile)daoResult.ReturnedObject;
            }
            catch(Exception exc)
            {
                await errorLogger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", userAccount.Username.ToString());
                result.IsSuccessful = false;
                result.Message = exc.Message;
            }
            return result;
        }

            public Result RegisterUser(string firstName, string lastName, string email, string password)
        {
            // System log entry recorded if registration process takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var userPersistResult = new Result();
            UserAccount userAccount = new UserAccount(firstName, lastName, email);

            // Password encryption
            HashSaltSecurity hashService = new HashSaltSecurity();
            string salt = BitConverter.ToString(hashService.GenerateSalt(password));
            string encryptedPass = hashService.GetHashSaltCredentials(password, salt);
            userAccount.Salt = salt;
            userPersistResult = _dao.PersistUser(userAccount, encryptedPass, salt);

            if (userPersistResult.IsSuccessful)
            {
                // Logging the registration
                successLogger.Log("Account created successfully", LogLevels.Info, "Data Store", userAccount.Username, new UserOperation(Operations.Registration, 1));
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error creating an account", LogLevels.Error, "Data Store", userAccount.Username);
            }

            stopwatch.Stop();
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);
            if (userPersistResult.IsSuccessful && actual > expected)
            {
                errorLogger.Log("Account created successfully, but took longer than 5 seconds", LogLevels.Info, "Business", userAccount.Username, new UserOperation(Operations.Registration, 1));
            }

            return userPersistResult;
        }

        public Result RegisterUser(string email, string password)
        {
            // System log entry recorded if registration process takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var userPersistResult = new Result();
            UserAccount userAccount = new UserAccount(email);

            // Password encryption
            HashSaltSecurity hashService = new HashSaltSecurity();
            string salt = BitConverter.ToString(hashService.GenerateSalt(password));
            string encryptedPass = hashService.GetHashSaltCredentials(password, salt);
            userAccount.Salt = salt;
            userPersistResult = _dao.PersistUser(userAccount, encryptedPass, salt);

            if (userPersistResult.IsSuccessful)
            {
                // Logging the registration
                successLogger.Log("Account created successfully", LogLevels.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error creating an account", LogLevels.Error, "Data Store", userAccount.Username);
            }

            stopwatch.Stop();
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);
            if(userPersistResult.IsSuccessful && actual > expected)
            {
                errorLogger.Log("Account created successfully, but took longer than 5 seconds", LogLevels.Info, "Business", userAccount.Username, new UserOperation(Operations.Registration, 1));
            }

            return userPersistResult;
        }

        public Result UpdateFirstName(UserAccount userAccount, string firstName)
        {
            string updateSQL = string.Format(@"UPDATE users SET FIRST_NAME = '{0}' WHERE username = '{1}'", firstName, userAccount.Username);
            return _dao.UpdateUser(userAccount, updateSQL);
        }

        public Result UpdateLastName(UserAccount userAccount, string lastName)
        {
            string updateSQL = string.Format(@"UPDATE users SET LAST_NAME = '{0}' WHERE username = '{1}'", lastName, userAccount.Username);
            return _dao.UpdateUser(userAccount, updateSQL);
        }

        public Result UpdateUsername(UserAccount userAccount, string username)
        {
            string updateSQL = string.Format(@"UPDATE users SET USERNAME = '{0}' WHERE username = '{1}'", username, userAccount.Username);
            return _dao.UpdateUser(userAccount, updateSQL);
        }

        public Result UpdatePassword(UserAccount userAccount, string password)
        {
            HashSaltSecurity hashService = new HashSaltSecurity();
            string salt = BitConverter.ToString(hashService.GenerateSalt(password));
            string encryptedPass = hashService.GetHashSaltCredentials(password, salt);
            userAccount.Salt = salt;

            string updateSQL = string.Format(@"UPDATE users SET PASSWORD = '{0}', SALT = '{1}' WHERE username = '{2}'", encryptedPass, salt, userAccount.Username);
            return _dao.UpdateUser(userAccount, updateSQL);
        }

        public Result UpdateUserIcon(UserAccount userAccount, int image)
        {
            string updateSQL = string.Format(@"UPDATE users SET IMAGE = '{0}' WHERE username = '{1}'", image, userAccount.Username);
            return _dao.UpdateUser(userAccount, updateSQL);
        }

        public Result UpdatePhoneNumber(UserAccount userAccount)
        {
            throw new NotImplementedException();
        }

        public Result UpdateNotifications(UserAccount userAccount)
        {
            string updateSQL = string.Format(@"UPDATE users SET NOTIFICATIONS = '{0}' WHERE username = '{1}'", JsonSerializer.Serialize(userAccount.Notifications), userAccount.Username);
            return _dao.UpdateUser(userAccount, updateSQL);
        }

        public Result DeleteUser(UserAccount userAccount)
        {
            Result deleteUserResult = _dao.DeleteUser(userAccount);
            if (deleteUserResult.IsSuccessful) {
                successLogger.Log("Account Deletion Successful", LogLevels.Info, "Data Store", userAccount.Username);
                deleteUserResult.Message = "Account Deletion Successful";
                deleteUserResult.ErrorStatus = HttpStatusCode.OK;
                return deleteUserResult;
            }
            else
            {
                errorLogger.Log("Account Deletion Unsuccessful", LogLevels.Error, "Data Store", userAccount.Username);
                deleteUserResult.Message = "Account Deletion Unsuccessful";
                deleteUserResult.ErrorStatus = HttpStatusCode.NotFound;
                return deleteUserResult;
            } 
        }

        public Result LogoutUser(UserAccount userAccount)
        {
            Result logoutResult = _dao.UpdateUserAuthentication(userAccount, false);
            if(logoutResult.IsSuccessful)
            {
                logoutResult.Message = "Successfully logged out.";
            }
            return logoutResult;
        }

        public Result EnableUser(UserAccount userAccount)
        {
            var enablingUser = new Result();
            // disabling user
            enablingUser = _dao.SetUserAbility(userAccount, 1);

            if (enablingUser.IsSuccessful)
            {
                // Logging the enabling user
                successLogger.Log("User enabled successfully", LogLevels.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error enabling user", LogLevels.Error, "Data Store", userAccount.Username);
            }
            return enablingUser;
        }

        public AuthResult ValidateAdminRole(UserAccount ua)
        {
            var result = new AuthResult();

            var serviceResult = _authService.ValidateAdminRole(ua);
            if(!serviceResult.IsSuccessful)
            {
                errorLogger.Log(serviceResult.Message, LogLevels.Error, "Service", ua.Username);
            }
            return serviceResult;
        }

    }
}
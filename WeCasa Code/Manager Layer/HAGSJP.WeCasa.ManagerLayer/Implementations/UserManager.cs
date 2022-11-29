using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.ManagerLayer.Implementations
{
    public class UserManager : IUserManager
    {
        public bool ValidateEmail(string email)
        {
            bool validEmail;
            var checkValidChar = new Regex(@"^[a-zA-Z0-9.,@!\- ]*$");
            if (!MailAddress.TryCreate(email, out var mailAddress))
            {
                validEmail = false;
            } 
            else if(email.Length > 255)
            {
                validEmail = false;
            } 
            else if(!checkValidChar.IsMatch(email))
            {
                validEmail = false;
            }
            else
            {
                validEmail = true;
            }
            return validEmail;
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
                    result.ErrorMessage = "Invalid Password: Password is not within the character range (8-80)";
                }
                else if (!checkUppercase.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid Password: Password does not contain upper case letter";
                }
                else if (!checkLowercase.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid Password: Password does not contain lower case letter";
                }
                else if (!checkNumber.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid Password: Password does not contain a numeric value";
                }
                else if (!checkSpecialChar.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid Password: Password does not contain a special case character";
                }
                else if (!checkValidChar.IsMatch(password))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid Password: Password contains invalid characters";
                }
                else
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "";
                }
                return result;
            }
        }

        public string ConfirmPassword(string password)
        {
            Console.WriteLine("Re-enter Password");
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

        public Result RegisterUser(string email, string password)
        {
            Result userPersistResult = new Result();

            User user = new User(email);
            MariaDbDAO dao = new MariaDbDAO();

            userPersistResult = dao.AddUser(user, password);

            if (userPersistResult.IsSuccessful)
            {
                // Logging the registration
                Logger successfulLogger = new Logger(dao);
                successfulLogger.Log("Account created successfully", LogLevel.Info, "Data Store", user.Email);
            }
            else
            {
                // Logging the error
                Logger errorLogger = new Logger(dao);
                errorLogger.Log("Error creating an account", LogLevel.Error, "Data Store", user.Email);

            }
            return userPersistResult;
        }
    }
}
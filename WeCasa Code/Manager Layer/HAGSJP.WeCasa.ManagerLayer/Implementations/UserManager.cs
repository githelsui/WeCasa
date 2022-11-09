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
        public string ValidateEmail()
        {
            Console.WriteLine("Enter Email Address: ");
            string email = Console.ReadLine();
            while (!MailAddress.TryCreate(email, out var mailAddress))
            {
                Console.WriteLine("Invalid email provided. Retry again or contact system administrator: ");
                email = Console.ReadLine();
            }
            return email;
        }
        public string ValidatePassword()
        {
            var checkNumber = new Regex(@"[0-9]+");
            var checkUppercase = new Regex(@"[A-Z]+");
            var checkLowercase = new Regex(@"[a-z]+");
            var checkLength = new Regex(@".{8,}");
            var checkSpecialChar = new Regex(@"[!@.,-]");

            Console.WriteLine("Enter Password: ");
            string password = Console.ReadLine();
            if (checkLength.IsMatch(password) && checkNumber.IsMatch(password) && checkUppercase.IsMatch(password) && checkLowercase.IsMatch(password) && checkSpecialChar.IsMatch(password))
            {
                return password;
            }

            else
            {
                bool validP = false;
                while (validP == false)
                {
                    Console.WriteLine("Invalid passphrase provided. Retry again or contact system administrator: ");
                    password = Console.ReadLine();
                    if (!checkLength.IsMatch(password))
                    {
                        Console.WriteLine("Invalid Password: Password is shorter than 8 Characters");
                    }
                    else if (!checkUppercase.IsMatch(password))
                    {
                        Console.WriteLine("Invalid Password: Password does not contain upper case letter");
                    }
                    else if (!checkLowercase.IsMatch(password))
                    {
                        Console.WriteLine("Invalid Password: Password does not contain lower case letter");
                    }
                    else if (!checkNumber.IsMatch(password))
                    {
                        Console.WriteLine("Invalid Password: Password does not contain a numeric value");
                    }
                    else if (!checkSpecialChar.IsMatch(password))
                    {
                        Console.WriteLine("Invalid Password: Password does not contain a special case character");
                    }
                    else
                    {
                        validP = true;
                    }
                }
                return password;
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
        public string GetUniqueUsername()
        {
            return "unique_username";
        }

        public Result RegisterUser(string email, string username, string password)
        {
            Result userPersistResult = new Result();

            User user = new User(username, email);
            MariaDbDAO dao = new MariaDbDAO();

            userPersistResult = dao.AddUser(user, password);

            if (userPersistResult.IsSuccessful)
            {
                // Logging the registration
                Logger successfulLogger = new Logger(dao);
                successfulLogger.Log("Account created successfully", "Info", "Data Store", user.Username);
            }
            else
            {
                // Logging the error
                Logger errorLogger = new Logger(dao);
                errorLogger.Log("Error creating an account", "Error", "Data Store", user.Username);

            }
            return userPersistResult;
        }
    }
}
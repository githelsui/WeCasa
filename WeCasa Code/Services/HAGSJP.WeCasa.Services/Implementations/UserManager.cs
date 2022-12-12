﻿using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly AccountMariaDAO _dao;
        private Logger successLogger;
        private Logger errorLogger;

        public UserManager() 
        {
            _dao = new AccountMariaDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public UserManager(AccountMariaDAO dao)
        {
            _dao = dao;
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }
        public bool ValidateEmail(string email)
        {
            bool validEmail;
            var checkValidChar = new Regex(@"^[a-zA-Z0-9.,@!\- ]*$");
            if (!MailAddress.TryCreate(email, out var mailAddress))
            {
                validEmail = false;
            }
            else if (email.Length > 255)
            {
                validEmail = false;
            }
            else if (!checkValidChar.IsMatch(email))
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
            foreach (var i in Enumerable.Range(0,10))
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
                successLogger.Log("One-time password generated successfully", LogLevel.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error saving a one-time password", LogLevel.Error, "Data Store", userAccount.Username);
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

        public Result RegisterUser(string email, string password)
        {
            var userPersistResult = new Result();
            UserAccount userAccount = new UserAccount(email);

            userPersistResult = _dao.PersistUser(userAccount, password);

            if (userPersistResult.IsSuccessful)
            {
                // Logging the registration
                successLogger.Log("Account created successfully", LogLevel.Info, "Data Store", userAccount.Username);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error creating an account", LogLevel.Error, "Data Store", userAccount.Username);
            }
            return userPersistResult;
        }

        public Result UpdateUser(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public Result DeleteUser()
        {
            throw new NotImplementedException();
        }

        public Result EnableUser()
        {
            throw new NotImplementedException();
        }

        public Result DisableUser()
        {
            throw new NotImplementedException();
        }
    }
}
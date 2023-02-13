using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Services.Implementations;
using System.Data;

namespace HAGSJP.WeCasa.Client
{
    public class RegistrationClient
    {
        private UserManager _um;

        public RegistrationClient()
        {
            _um = new UserManager();
        }

        // Used in Web API
        public Result Register(string firstName, string lastName, string email, string password)
        {
            var registerResult = new Result();

            // Input Validation
            var emailTaken = _um.IsUsernameTaken(email);
            var validateEmail = _um.ValidateEmail(email);
            var validatePassword = _um.ValidatePassword(password);

            if (!emailTaken && validateEmail.IsSuccessful && validatePassword.IsSuccessful)
            {
                registerResult = _um.RegisterUser(firstName, lastName, email, password);
            }
            else
            {
                var emailTakenMessage = emailTaken ? "An account already exists with this email." : "";
                registerResult.Message = validateEmail.Message + "\n" + validatePassword.Message + "\n" + emailTakenMessage;
                registerResult.IsSuccessful = false;
            }
            return registerResult;
        }

        public Result Register(string email, string password, UserManager um)
        {
            var registerResult = new Result();
            if (um.ValidateEmail(email).IsSuccessful && !um.IsUsernameTaken(email))
            {
                registerResult = um.RegisterUser(email, password);
                if (registerResult.IsSuccessful)
                {
                    registerResult.Message = "Account created successfully!\n";
                    // Create User Account
                    UserAccount userAccount = new UserAccount(email);
                    // Providing username to the user
                    Console.WriteLine("Username: " + email + "\n");
                }
                else
                {
                    registerResult.Message = "An error occurred. Please try again later.";
                }
            }
            else
            {
                registerResult.IsSuccessful = false;
                registerResult.Message = ("Invalid username or password provided. Retry again or contact the System Administrator.");
            }
            return registerResult;
        }
    }
}
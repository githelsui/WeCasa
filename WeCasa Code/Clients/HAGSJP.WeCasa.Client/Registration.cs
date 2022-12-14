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
    public class Registration
    {
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
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Client
{
    public class Registration
    {
        public string GetEmail(UserManager um)
        {
            Console.WriteLine("Enter Email Address: ");
            string email = Console.ReadLine();
            bool validEmail = um.ValidateEmail(email);
            while (!validEmail || um.IsUsernameTaken(email))
            {
                Console.WriteLine("Invalid username or password provided. Retry again or contact system administrator.");
                email = Console.ReadLine();
                validEmail = um.ValidateEmail(email);
            }
            return email;
        }
        public void Register(string email, string password, UserManager um)
        {
            var registerResult = um.RegisterUser(email, password);
            if (registerResult.IsSuccessful)
            {
                Console.WriteLine("Account created successfully!\n");
                // Create User Account
                UserAccount userAccount = new UserAccount(email);
                // Providing username to the user
                Console.WriteLine("Username: " + email);
            }
            else
            {
                Console.WriteLine("An error occurred. Please try again later.");
            }
        }
    }
}
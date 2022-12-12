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

        public void Register(String email, String password, UserManager um)
        {
            var registerResult = um.RegisterUser(email, password);
            if (registerResult.IsSuccessful)
            {
                Console.WriteLine("Account created successfully!\n");
                // Create User Account
                UserAccount userAccount = new UserAccount(email);
                // Providing username and OTP to the user
                OTP otp = um.GenerateOTPassword(userAccount);
                Console.WriteLine("Username: " + email);
                Console.WriteLine("One-time login code: " + otp.Code);
                Console.WriteLine("Your one-time code will expire at " + otp.CreateTime.AddMinutes(2) + "\n");
            }
            else
            {
                Console.WriteLine("An error occurred. Please try again later.");
            }
        }
    }
}
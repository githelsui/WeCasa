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
            while (!validEmail)
            {
                Console.WriteLine("Invalid email provided. Retry again or contact system administrator: ");
                email = Console.ReadLine();
                validEmail = um.ValidateEmail(email);
            }
            return email;
        }

        public string GetPassword(UserManager um)
        {
            Console.WriteLine("Enter Password: ");
            string password = Console.ReadLine();
            var validP = new Result();
            validP = um.ValidatePassword(password);
            while (validP.IsSuccessful == false)
            {
                Console.WriteLine(validP.Message);
                password = Console.ReadLine();
                validP = um.ValidatePassword(password);
            }
            return password;
        }

        public void RegisterUser(UserManager um)
        {
            string email = GetEmail(um);
            // check if user already exists, is authenticated
            string password = GetPassword(um);
            var validPW = um.ValidatePassword(password);
            string confirmPassword = um.ConfirmPassword(password);
            var registerResult = um.RegisterUser(email, password);
            if (validPW.IsSuccessful && registerResult.IsSuccessful)
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

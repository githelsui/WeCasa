using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Client
{
    class Menu
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

        public void OpenMenu()
        {
            bool menu = true;
            while (menu != false)
            {
                Console.WriteLine("(1) Register New Account");
                Console.WriteLine("(2) Login to Existing Account");
                Console.WriteLine("(3) Exit");
                UserManager um = new UserManager();
                switch (Console.ReadLine())
                {
                    case "1":
                        string email = GetEmail(um);
                        // check if user already exists, is authenticated
                        string password = GetPassword(um);
                        var validPW = um.ValidatePassword(password);
                        var registerResult = um.RegisterUser(email, password);
                        if (validPW.IsSuccessful && registerResult.IsSuccessful)
                        {
                            Console.WriteLine("Account Created!");
                            // Create User Account
                            UserAccount userAccount = new UserAccount(email);
                            // Providing username and OTP to the user
                            OTP otp = um.GenerateOTPassword(userAccount);
                            Console.WriteLine("Username: ", email);
                            Console.WriteLine("One-time login code: ", otp.Code);
                            Console.WriteLine("Your one-time code will expire at ", otp.CreateTime.AddMinutes(2));
                        }
                        else
                        {
                            Console.WriteLine("An error occurred. Please try again later.");
                        }
                        break;
                    case "2": // TODO
                        Login login = new Login();
                        login.LoginUser();
                        // Creating User Profile
                        break;
                    case "3":
                        menu = false;
                        break;
                }
            }
        }
    }
}
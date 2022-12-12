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
                Console.WriteLine("Invalid username or password provided. Retry again or contact system administrator.");
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

        public OTP GetOTP(string email, Authentication auth)
        {
            OTP otp;
            Console.WriteLine("Enter one-time code: ");
            string code = Console.ReadLine();
            var validP = new Result();
            validP = auth.VerifyOTPassword(email, code);
            while (validP.IsSuccessful == false)
            {
                Console.WriteLine(validP.Message);
                code = Console.ReadLine();
                validP = auth.VerifyOTPassword(email, code);
            }
            otp = new OTP(email, code);
            return otp;
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
                string email;
                string password;
                switch (Console.ReadLine())
                {
                    case "1":
                        Registration reg = new Registration();
                        email = GetEmail(um);
                        // check if user already exists
                        password = GetPassword(um);
                        string confirmPassword = um.ConfirmPassword(password);
                        reg.Register(email, password, um);
                        break;
                    case "2":
                        Authentication auth = new Authentication();
                        Login login = new Login();
                        // Get username and password from commandline
                        email = GetEmail(um);
                        password = GetPassword(um);
                        OTP otp = GetOTP(email, auth);
                        // Fetch all user info from database
                        
                        login.LoginUser(password, otp, auth);
                        break;
                    case "3":
                        menu = false;
                        break;
                }
            }
        }
    }
}
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
using static System.Net.WebRequestMethods;

namespace HAGSJP.WeCasa.Client
{
    class Menu
    {
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

        public OTP GetOTP(UserAccount userAccount, Authentication auth)
        {
            OTP otp;
            Console.WriteLine("Enter one-time code: ");
            string code = Console.ReadLine();
            var validP = new Result();
            validP = auth.VerifyOTPassword(userAccount.Username, code);
            while (validP.IsSuccessful == false)
            {
                Console.WriteLine(validP.Message);
                code = Console.ReadLine();
                validP = auth.VerifyOTPassword(userAccount.Username, code);
            }
            otp = new OTP(userAccount.Username, code);
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
                UserAccount ua;
                string email;
                string password;
                switch (Console.ReadLine())
                {
                    case "1":
                        Registration reg = new Registration();
                        email = reg.GetEmail(um);
                        password = GetPassword(um);
                        string confirmPassword = um.ConfirmPassword(password);
                        reg.Register(email, password, um);
                        break;
                    case "2":
                        Authentication auth = new Authentication();
                        Login login = new Login();
                        // Get username and password from commandline
                        email = login.GetEmail(um);
                        password = GetPassword(um);
                        ua = new UserAccount(email, password);
                        // Checking pre-conditions for login
                        var result = login.CheckUser(ua, auth, um);
                        if (result.IsSuccessful)
                        { 
                            // Generating one-time code 
                            OTP otp = um.GenerateOTPassword(ua);
                            Console.WriteLine("One-time login code: " + otp.Code);
                            Console.WriteLine("Your one-time code will expire at " + otp.CreateTime.AddMinutes(2) + "\n");
                            // Getting OTP from user
                            OTP inputOtp = GetOTP(ua, auth);
                            login.LoginUser(ua, inputOtp, auth);
                            // Going to home page
                            menu = false;
                        }
                        // User is unable to log in
                        else
                        {
                            // Displaying user-friendly error message
                            Console.WriteLine(result.Message);
                        }
                        break;
                    case "3":
                        menu = false;
                        break;
                }
            }
        }
    }
}
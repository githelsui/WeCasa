using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Client
{
    class Login
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
        public Result CheckUser(UserAccount userAccount, Authentication auth, UserManager um)
        {
            // Checking if the user exists, make sure they are not already authenticated
            var result = auth.IsExistingAccount(userAccount);
            return result;
        }
        public void LoginUser(UserAccount userAccount, OTP otp, Authentication auth)
        {
            // Checking OTP, creating a new authentication session for the user
            var loginResult = auth.AuthenticateUser(userAccount, otp);

            if (loginResult.IsSuccessful)
            {
                Console.WriteLine(otp.Username, " logged in...");
            }
            else
            {
                Console.WriteLine(loginResult.Message);
            }
        }
    }
}
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
        public void LoginUser(string password, OTP otp, Authentication auth)
        {
            // check if the user exists, make sure they are not already authenticated
            var loginResult = auth.AuthenticateUser(password, otp);

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
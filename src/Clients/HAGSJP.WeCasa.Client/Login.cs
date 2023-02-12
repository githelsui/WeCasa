using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Client
{
    public class Login
    {
        private UserManager _um;
        private Authentication _auth;

        public Login()
        {
            _um = new UserManager();
            _auth = new Authentication(); 
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

        public OTP GetOTP(UserAccount userAccount)
        {
            return _um.GenerateOTPassword(userAccount);
        }

        public Result SubmitOTP(UserAccount userAccount, string code)
        {
            return _auth.VerifyOTPassword(userAccount.Username, code);
        }

        public Result ValidateEncryptedPasswords(UserAccount userAccount, Authentication auth)
        {
            // Checking if the user exists, make sure they are not already authenticated
            var validationResult = auth.VerifyEncryptedPasswords(userAccount);
            return validationResult;
        }

        //Prior to receiving an OTP Code: validate preconditions and encrypted passwords
        public Result AttemptLogin(UserAccount userAccount)
        {
            // Checking pre-conditions for login
            var loginResult = new Result();
            var validEmail = _um.ValidateEmail(userAccount.Username);
            if (!validEmail.IsSuccessful)
            {
                loginResult.IsSuccessful = false;
                loginResult.Message = "Invalid email provided. Retry again or contact the System Administrator.";
            }
            bool validPassword = _um.ValidatePassword(userAccount.Password).IsSuccessful;
            if (!validPassword)
            {
                loginResult.IsSuccessful = false;
                loginResult.Message = "Invalid password provided. Retry again or contact the System Administrator.";
            }
            var correctPassword = ValidateEncryptedPasswords(userAccount, _auth);
            if (correctPassword.IsSuccessful)
            {
                loginResult.IsSuccessful = true;
                loginResult.Message = "Login attempt successful.";
            }
            return loginResult;
        }

        public Result LoginUser(UserAccount userAccount, Authentication auth, UserManager userManager)
        {
            // Checking pre-conditions for login
            var loginResult = new Result();
            var validEmail = userManager.ValidateEmail(userAccount.Username);
            if  (!validEmail.IsSuccessful)
            {
                loginResult.IsSuccessful = false;
                loginResult.Message = "Invalid email provided. Retry again or contact the System Administrator.";
            }
            bool validPassword = userManager.ValidatePassword(userAccount.Password).IsSuccessful;
            if (!validPassword)
            {
                loginResult.IsSuccessful = false;
                loginResult.Message = "Invalid email provided. Retry again or contact the System Administrator.";
            }
            loginResult = ValidateEncryptedPasswords(userAccount, auth);
            if (loginResult.IsSuccessful)
            {
                // Generating one-time code 
                OTP otp = userManager.GenerateOTPassword(userAccount);
                Console.WriteLine("One-time login code: " + otp.Code);
                Console.WriteLine("Your one-time code will expire at " + otp.CreateTime.AddMinutes(2) + "\n");
                // Getting OTP from user
                OTP inputOtp = GetOTP(userAccount, auth);
                // Checking OTP, creating a new authentication session for the user
                loginResult = auth.AuthenticateUser(userAccount, inputOtp, otp);
                return loginResult;
            }
            else
            {
                return loginResult;
            }
        }
    }
}
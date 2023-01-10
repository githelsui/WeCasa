using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Abstractions;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class HashSaltSecurity : IHashSaltSecurity
    {
        private PasswordHasher<UserAccount> PwHasher = new PasswordHasher<UserAccount>();

        public HashSaltSecurity()
        {
            PwHasher = new PasswordHasher<UserAccount>();
        }

        public byte[] GenerateSalt(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            return salt;
        }

        public string GetHashSaltCredentials(string password, string salt)
        {
            return PwHasher.HashPassword(password, salt);
        }

        public AuthResult ValidateHashedPasswords(UserAccount userAccount, AuthResult result)
        {
            string providedPassword = userAccount.Password;
            string savedEncryptedPass = result.ReturnedObject.ToString();
            PasswordVerificationResult verification = PwHasher.VerifyHashedPassword(userAccount, savedEncryptedPass, providedPassword);

            if (verification.Equals(PasswordVerificationResult.Success))
            {
                result.IsSuccessful = true;
                result.HasValidCredentials = true;
                result.Message = "Authentication successful.";
            }
            else if (verification.Equals(PasswordVerificationResult.Failed)) // Invalid credentials
            {
                result.IsSuccessful = false;
                result.HasValidCredentials = false;
                result.Message = "Invalid username or password provided. Retry again or contact the System Administrator.";
            }
            else //PasswordVerificationResult.SuccessRehashNeeded
            {
                result.IsSuccessful = false;
                result.HasValidCredentials = true;
                result.Message = "Password verification was successful however the password was encoded using a deprecated algorithm and should be rehashed and updated.";
            }
            return result;
        }
    }
}

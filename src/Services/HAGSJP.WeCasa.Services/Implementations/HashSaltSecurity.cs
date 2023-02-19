using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Abstractions;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace HAGSJP.WeCasa.Services.Implementations
{
	public class HashSaltSecurity : IHashSaltSecurity
	{
        
        public HashSaltSecurity() { }

        public byte[] GenerateSalt(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            return salt;
        }

        public string GetHashSaltCredentials(string password)
		{
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
		}

        public string GetHashSaltCredentials(string password, string salt)
		{
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public AuthResult ValidateHashedPasswords(UserAccount userAccount, AuthResult result)
        {
            string salt = userAccount.Salt;
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            string newEncryptedPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userAccount.Password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            string savedEncryptedPass = result.ReturnedObject.ToString();

            if (newEncryptedPass.Equals(savedEncryptedPass))
            {
                result.IsSuccessful = true;
                result.HasValidCredentials = true;
                result.Message = "Authentication successful.";
            }
            else // Invalid credentials
            {
                result.IsSuccessful = false;
                result.HasValidCredentials = false;
                result.Message = "Invalid username or password provided. Retry again or contact the System Administrator.";
            }
            return result;
        }
    }
}


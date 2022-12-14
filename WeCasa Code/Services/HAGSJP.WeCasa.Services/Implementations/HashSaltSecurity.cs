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
        private readonly HashDAO _dao;
        private readonly AccountMariaDAO _logDao;
        private Logger successLogger;
        private Logger errorLogger;

        public HashSaltSecurity()
		{
            _dao = new HashDAO();
            _logDao = new AccountMariaDAO();
            successLogger = new Logger(_logDao);
            errorLogger = new Logger(_logDao);
        }

        public HashSaltSecurity(HashDAO dao)
        {
            _dao = dao;
            _logDao = new AccountMariaDAO();
            successLogger = new Logger(_logDao);
            errorLogger = new Logger(_logDao);
        }

        public string GetHashSaltCredentials(string password)
		{
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

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

        public AuthResult ValidateHashedPasswords(string username, string password)
        {
            var result = new AuthResult();

            AuthResult saltResult = _dao.GetSalt(username);
            if (saltResult.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(saltResult.Message, LogLevels.Error, "Data Store", username);
                return saltResult;
            }
            var salt = saltResult.ReturnedObject.ToString();
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            string newEncryptedPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            AuthResult encryptedPasswordResult = _dao.GetEncryptedPassword(username);
            if (encryptedPasswordResult.IsSuccessful == false)
            {
                // Failure case from data store layer
                errorLogger.Log(encryptedPasswordResult.Message, LogLevels.Error, "Data Store", username);
                return saltResult;
            }
            var savedEncryptedPass = encryptedPasswordResult.ReturnedObject.ToString();

            if (newEncryptedPass.Equals(savedEncryptedPass))
            {
                result.IsSuccessful = true;
                result.ReturnedObject = true;
            } else
            {
                result.IsSuccessful = true;
                result.ReturnedObject = false;
            }

            return result;
        }

        public AuthResult ValidateHashedPasswords(UserAccount ua)
        {
            return ValidateHashedPasswords(ua.Username, ua.Password);
        }
    }
}


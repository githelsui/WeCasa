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
    }
}


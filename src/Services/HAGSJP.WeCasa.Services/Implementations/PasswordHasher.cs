using System;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
using HAGSJP.WeCasa.Models;
using Microsoft.AspNetCore.Identity;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : UserAccount
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int Iterations = 10000;

        public PasswordHasher()
        {
            Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Identity.PasswordHasherOptions>? optionsAccessor = default;
        }

        public byte[] GenerateSalt(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            return salt;
        }

        public string HashPassword(string password, string saltStr)
        {
            byte[] salt;
            byte[] buffer;
            // Encrypt provided password
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, SaltByteSize, Iterations))
            {
                salt = bytes.Salt; ;
                buffer = bytes.GetBytes(HashByteSize);
            }
            byte[] dst = new byte[(SaltByteSize + HashByteSize) + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);

        }

        //IPasswordHasher Interface Methods
        public string HashPassword(TUser userAccount, string password)
        {
            byte[] saltBytes = GenerateSalt(password);
            byte[] salt;
            byte[] buffer;
            // Encrypt provided password
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, SaltByteSize, Iterations))
            {
                salt = bytes.Salt; ;
                buffer = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[(SaltByteSize + HashByteSize) + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);
        }

        //IPasswordHasher Interface Methods
        public PasswordVerificationResult VerifyHashedPassword(TUser userAccount, string hashedPassword, string providedPassword)
        {
            int arrayLen = (SaltByteSize + HashByteSize) + 1;
            byte[] provdedHashedPassword;
            byte[] salt;
            byte[] buffer;

            // Decrpyt data
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != arrayLen) || (src[0] != 0))
            {
                return PasswordVerificationResult.Failed;
            }

            // Stored salt data from db
            byte[] currentSaltBytes = new byte[SaltByteSize];
            Buffer.BlockCopy(src, 1, currentSaltBytes, 0, SaltByteSize);

            // Stored password data from db
            byte[] currentHashBytes = new byte[HashByteSize];
            Buffer.BlockCopy(src, SaltByteSize + 1, currentHashBytes, 0, HashByteSize);

            // Encrypt provided password
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(providedPassword, currentSaltBytes, Iterations))
            {
                provdedHashedPassword = bytes.GetBytes(HashByteSize);
            }

            if (ByteArraysEqual(currentHashBytes, provdedHashedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            else // Invalid credentials
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private static bool ByteArraysEqual(byte[] firstHash, byte[] secondHash)
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
}

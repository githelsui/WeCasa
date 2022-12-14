using HAGSJP.WeCasa.Models;
using System;
namespace HAGSJP.WeCasa.Services.Abstractions
{
	public interface IHashSaltSecurity
	{
		public string GetHashSaltCredentials(string password);
		public string GetHashSaltCredentials(string password, string salt);
		public byte[] GenerateSalt(string password);
		public AuthResult ValidateHashedPasswords(UserAccount userAccount, AuthResult authResult);
    }
}


using System;
namespace HAGSJP.WeCasa.Services.Abstractions
{
	public interface IHashSaltSecurity
	{
		public string GetHashSaltCredentials(string password);
		public string GetHashSaltCredentials(string password, string salt);
		public byte[] GenerateSalt(string password);

    }
}


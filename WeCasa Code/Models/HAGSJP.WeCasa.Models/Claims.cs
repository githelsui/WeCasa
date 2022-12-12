using System;
using System.Text.Json;

namespace HAGSJP.WeCasa.Models
{
	public class Claims
	{
		public Claims(string jsonClaims)
		{
			UserClaims = DeserializeUsingGenericSystemTextJson(jsonClaims);
		}

        public Claims()
        {
            UserClaims = new List<Claim>();
        }

        private List<Claim> DeserializeUsingGenericSystemTextJson(string jsonClaims)
        {
            List<Claim> claims = JsonSerializer.Deserialize<List<Claim>>(jsonClaims);
            return claims;
        }

        public List<Claim> UserClaims { get; set; }
        public string? Username { get; set; }
    }
}


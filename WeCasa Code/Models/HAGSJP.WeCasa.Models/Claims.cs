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

        private Claim DeserializeUsingGenericSystemTextJson(string json)
        {
            var claims = JsonSerializer.Deserialize<Claim>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return claims;
        }

        public Claim UserClaims { get; set; }
	}
}


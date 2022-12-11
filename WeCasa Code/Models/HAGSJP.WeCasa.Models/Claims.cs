using System;
using System.Text.Json;
using Newtonsoft.Json;

namespace HAGSJP.WeCasa.Models
{
	public class Claims
	{
		public Claims(string jsonClaims)
		{
			UserClaims = DeserializeUsingGenericSystemTextJson(jsonClaims);
		}

        private List<Claim> DeserializeUsingGenericSystemTextJson(string jsonClaims)
        {
            List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(jsonClaims);
            return claims;
        }

        public List<Claim> UserClaims { get; set; }
	}
}


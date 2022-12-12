using System;
using System.Text.Json;

namespace HAGSJP.WeCasa.Models.Security
{
    public class Claims
    {
        public Claims() { }
        public Claims(string jsonClaims)
        {
            UserClaims = DeserializeUsingGenericSystemTextJson(jsonClaims);
        }

        private List<Claim> DeserializeUsingGenericSystemTextJson(string jsonClaims)
        {
            List<Claim> claims = JsonSerializer.Deserialize<List<Claim>>(jsonClaims);
            return claims;
        }

        public List<Claim> UserClaims { get; set; }
    }
}


namespace HAGSJP.WeCasa.Models
{
	public class Claim
	{
		public Claim(string claimType, string claimValue)
		{
			ClaimType = claimType;
			ClaimValue = claimValue;
		}

		public string ClaimType { get; set; }
		public string ClaimValue { get; set; }

    }
}


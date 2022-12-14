using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Models
{
    public interface Event
	{
        public UserAccount user { get; set; }
        public string isAdmin { get; set; }
        public List<Claim> claims { get; set; }
        public List<DateTime> failedAttemptTimes { get; set; }
	}
}


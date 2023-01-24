using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Models
{
    public class UserStatus : UserAccount
    {
        public UserStatus(string email, string password, int userAccountId, bool isEnabled, bool isAuth, bool isAdmin, string otpCode, DateTime otpTime, List<Claim> claims)
        {
            this.Username = email;
            this.Password = password;
            this.UserAccountId = userAccountId;
            this.IsEnabled = isEnabled;
            this.IsAuth = isAuth;
            this.IsAdmin = isAdmin;
            this.OtpCode = otpCode;
            this.OtpTime = otpTime;
            this.Claims = claims;
        }

        public bool IsEnabled { get; set; }
        public bool IsAuth { get; set; }
        public bool IsAdmin { get; set; }
        public string OtpCode { get; set; }
        public DateTime OtpTime { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
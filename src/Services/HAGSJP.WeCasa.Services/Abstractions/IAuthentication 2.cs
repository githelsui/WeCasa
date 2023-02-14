using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IAuthentication
    {
        public Result VerifyOTPassword(string email, string code);
        public AuthResult VerifyEncryptedPasswords(UserAccount userAccount);
        public Result AuthenticateUser(UserAccount userAccount, OTP submittedOTP);
        public Result AuthenticateUser(UserAccount userAccount, OTP userOtp, OTP otp);
        public Boolean IsAccountEnabled(UserAccount userAccount);
        public Result ResetAuthenticationAttempts(UserAccount userAccount);
    }
}
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IAuthentication
    {
        public Result VerifyOTPassword(string email, string code);
        public Result AuthenticateUser(string password, OTP otp);
    }
}
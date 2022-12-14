﻿using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IAuthentication
    {
        public Result VerifyOTPassword(string email, string code);
        public AuthResult VerifyEncryptedPasswords(string email, string password);
        public Result AuthenticateUser(UserAccount userAccount, OTP otp);
        public Boolean IsAccountEnabled(UserAccount userAccount);
        public Result ResetAuthenticationAttempts(UserAccount userAccount);
    }
}
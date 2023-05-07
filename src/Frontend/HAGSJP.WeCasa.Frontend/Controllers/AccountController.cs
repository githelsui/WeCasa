using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using Microsoft.IdentityModel.Tokens;

namespace HAGSJP.WeCasa.Frontend.Controllers
{

    [ApiController]
    [Route("account-settings")]
    [Route("account-recovery")]
    public class AccountController : ControllerBase
    {

        [HttpPost]
        [Route("UpdateNotifications")]
        public Result UpdateNotifications([FromBody] AccountForm form)
        {
            UserAccount ua = new UserAccount();
            ua.Username = form.Email;
            ua.Notifications = form.Notifications;
            UserManager um = new UserManager();
            return um.UpdateNotifications(ua);
        }

        [HttpPost]
        [Route("UpdateEmail")]
        public Result UpdateEmail([FromBody] AccountForm form)
        {
            UserAccount ua = new UserAccount(form.Email);
            string newUsername = form.NewField;
            UserManager um = new UserManager();
            return um.UpdateUsername(ua, newUsername);
        }

        [HttpPost]
        [Route("UpdatePassword")]
        public Result UpdatePassword([FromBody] AccountForm form)
        {
            UserAccount ua = new UserAccount(form.Email, form.Password);
            string newPassword = form.NewField;
            UserManager um = new UserManager();
            return um.UpdatePassword(ua, newPassword);
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public Result DeleteAccount([FromBody] LoginForm form)
        {
            UserAccount ua = new UserAccount(form.Username);
            UserManager um = new UserManager();
            return um.DeleteUser(ua);
        }

        [HttpPost]
        [Route("GetOTPCode")]
        public async Task<AuthResult> GetOTPCode([FromBody] AccountForm form)
        {
            UserAccount ua = new UserAccount(form.Email);
            UserManager um = new UserManager();
            var result = await um.GetOTRecoveryCode(ua);          
            return result;
        }

        [HttpPost]
        [Route("VerifyOTPCode")]
        public async Task<AuthResult> VerifyOTPCode([FromBody] AccountForm form)
        {
            UserAccount ua = new UserAccount(form.Email);
            OTP otp = new OTP(form.Email, form.OTP.ToString());
            UserManager um = new UserManager();
            var result = await um.VerifyOTRecoveryCode(ua, otp);
            return result;
        }

        [HttpPost]
        [Route("UpdatePasswordSecured")]
        public Result UpdatePasswordSecured([FromBody] AccountForm form)
        {
            UserAccount ua = new UserAccount(form.Email);
            string newPassword = form.NewField;
            UserManager um = new UserManager();
            return um.UpdatePassword(ua, newPassword);
        }
    }
}
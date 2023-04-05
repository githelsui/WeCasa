using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IUserManager
    {
        public Result ValidateName(string name);
        public Result ValidateEmail(string email);
        public Result ValidatePassword(string Password);
        public bool IsUsernameTaken(string username);
        public OTP GenerateOTPassword(UserAccount userAccount);
        public string ConfirmPassword(string password);
        public UserStatus PopulateUserStatus(UserAccount userAccount);
        public AuthResult GetUserProfile(UserAccount userAccount);
        public Result RegisterUser(string firstName, string lastName, string email, string password);
        public Result RegisterUser(string email, string password);
        public Result DeleteUser(UserAccount userAccount);
        public Result UpdateFirstName(UserAccount userAccount, string firstName);
        public Result UpdateLastName(UserAccount userAccount, string lastName);
        public Result UpdateUsername(UserAccount userAccount, string userName);
        public Result UpdatePassword(UserAccount userAccount, string Password);
        public Result UpdateUserIcon(UserAccount userAccount, int image);
        public Result UpdatePhoneNumber(UserAccount userAccount);
        public Result LogoutUser(UserAccount userAccount);
        public Result EnableUser(UserAccount userAccount);
        public Result DisableUser(UserAccount userAccount);
    }
}
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Principal;

namespace HAGSJP.WeCasa.ManagerLayer.Implementations
{
    public interface IUserManager
    {
        public bool ValidateEmail(string email);
        public Result ValidatePassword(string Password);
        public string ConfirmPassword(string password);
        public Result RegisterUser(string email, string password);
        public Result DeleteUser();
        public Result UpdateUser(UserProfile userProfile);
        public Result EnableUser();
        public Result DisableUser();
    }
}
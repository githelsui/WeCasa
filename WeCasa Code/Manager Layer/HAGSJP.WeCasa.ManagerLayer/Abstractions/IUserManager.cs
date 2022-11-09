using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.ManagerLayer.Implementations
{
    public interface IUserManager
    {
        public string ValidateEmail();
        public string ValidatePassword();
        public string ConfirmPassword(string password);
        public Result RegisterUser(string email, string password);
    }
}
using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Client
{
	public class Logout
	{
        private UserManager _um;

        public Logout()
        {
            _um = new UserManager();
        }
        public Result LogoutUser(UserAccount userAccount)
        {
            return _um.LogoutUser(userAccount);
        }
    }
}


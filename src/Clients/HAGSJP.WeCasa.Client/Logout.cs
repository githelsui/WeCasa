using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Client
{
	public class Logout
	{
        public Result LogoutUser(UserAccount userAccount)
        {
            UserManager userManager = new UserManager();
            return userManager.LogoutUser(userAccount);
        }
    }
}


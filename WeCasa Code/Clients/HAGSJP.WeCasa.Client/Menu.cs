using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Client
{
    class Menu
    { 

        public void OpenMenu()
        {
            bool menu = true;
            while (menu != false)
            {
                Console.WriteLine("(1) Register New Account");
                Console.WriteLine("(2) Login to Existing Account");
                Console.WriteLine("(3) Exit");
                UserManager um = new UserManager();
                switch (Console.ReadLine())
                {
                    case "1":
                        Registration register = new Registration();
                        register.RegisterUser(um);
                        break;
                    case "2": // TODO
                        Login login = new Login();
                        login.LoginUser(um);
                        // Creating User Profile
                        break;
                    case "3":
                        menu = false;
                        break;
                }
            }
        }
    }
}
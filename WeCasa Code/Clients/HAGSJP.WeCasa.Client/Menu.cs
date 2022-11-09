using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                //Console.WriteLine("(2) Login to Existing Account");
                Console.WriteLine("(2) Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        Registration r = new Registration();
                        string email = r.ValidateEmail();
                        string password = r.ValidatePassword();
                        string confirmPassword = r.ConfirmPassword(password);
                        string username = r.GetUniqueUsername();
                        r.RegisterUser(email, username, password);
                        break;
                    /*case "2":
                        Login l = new Login();
                        break;*/
                    case "2":
                        menu = false;
                        break;
                }
            }
        }
    }
}
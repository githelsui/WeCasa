using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.ManagerLayer.Implementations;

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
                        UserManager um = new UserManager();
                        string email = um.ValidateEmail();
                        string password = um.ValidatePassword();
                        string confirmPassword = um.ConfirmPassword(password);
                        var result = um.RegisterUser(email, password);
                        if (result.IsSuccessful)
                        {
                            Console.WriteLine("Account Created!");
                        } else
                        {
                            Console.WriteLine("An error occurred. Please try again later.");
                        }
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
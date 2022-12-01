using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.ManagerLayer.Implementations;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Client
{
    class Menu
    { 
        public string GetEmail(UserManager um)
        {
            Console.WriteLine("Enter Email Address: ");
            string email = Console.ReadLine();
            bool validEmail = um.ValidateEmail(email);
            while (!validEmail)
            {
                Console.WriteLine("Invalid email provided. Retry again or contact system administrator: ");
                email = Console.ReadLine();
                validEmail = um.ValidateEmail(email);
            }
            return email;
        }

        public string GetPassword(UserManager um)
        {
            Console.WriteLine("Enter Password: ");
            string password = Console.ReadLine();
            var validP = new Result();
            validP = um.ValidatePassword(password);
            while (validP.IsSuccessful == false)
            {
                Console.WriteLine(validP.Message);
                password = Console.ReadLine();
                validP = um.ValidatePassword(password);
            }
            return password;
        }

        public void OpenMenu()
        {
            bool menu = true;
            while (menu != false)
            {
                Console.WriteLine("(1) Register New Account");
                Console.WriteLine("(2) Login to Existing Account");
                Console.WriteLine("(3) Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        UserManager um = new UserManager();
                        string email = GetEmail(um);
                        string password = GetPassword(um);
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
                    case "2":
                        Login l = new Login();
                        break;
                    case "3":
                        menu = false;
                        break;
                }
            }
        }
    }
}
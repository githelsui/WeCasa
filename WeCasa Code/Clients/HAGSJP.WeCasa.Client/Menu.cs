//using HAGSJP.WeCasa.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace HAGSJP.WeCasa.Client
//{
class Menu
{
    static void Main(string[] args)
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
                    Registration r = new Registration();
                    string email = r.ValidateEmail();
                    string password = r.ValidatePassword();
                    string confirmPassword = r.ConfirmPassword(password);
                    Console.WriteLine("Account Created!");
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
//}
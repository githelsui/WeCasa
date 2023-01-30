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
using static System.Net.WebRequestMethods;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Client
{
    class Menu
    {
        public string GetEmail(UserManager um)
        {
            Console.WriteLine("Enter Email Address: ");
            string email = Console.ReadLine();
            var validEmail = um.ValidateEmail(email);
            while (!validEmail.IsSuccessful)
            {
                Console.WriteLine(validEmail.Message);
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
                Console.WriteLine("(3) Logout");
                Console.WriteLine("(4) Exit");
                UserManager um = new UserManager();
                UserAccount ua;
                string email;
                string password;
                switch (Console.ReadLine())
                {
                    case "1":
                        RegistrationClient reg = new RegistrationClient();
                        email = GetEmail(um);
                        password = GetPassword(um);
                        string confirmPassword = um.ConfirmPassword(password);
                        var regResult = reg.Register(email, password, um);
                        Console.WriteLine(regResult.Message);
                        break;
                    case "2":
                        Authentication auth = new Authentication();
                        Login login = new Login();
                        // Get username and password from commandline
                        email = GetEmail(um);
                        password = GetPassword(um);
                        ua = new UserAccount(email, password);
                        var loginResult = login.LoginUser(ua, auth, um);
                        if(loginResult.IsSuccessful)
                        {
                            // Going to home page
                            Home h = new Home();
                            h.HomePage(ua);
                            menu = false;
                        } 
                        // User is unable to log in
                        else
                        {
                            // Displaying user-friendly error message
                            Console.WriteLine(loginResult.Message);
                        }
                        break;
                    case "3":
                        Logout logout = new Logout();
                        Console.Write("Enter credentials for account to logout.\n\n");
                        email = GetEmail(um);
                        password = GetPassword(um);
                        ua = new UserAccount(email, password);
                        var logoutRes = logout.LogoutUser(ua);
                        if (logoutRes.IsSuccessful)
                        {
                            Console.Write("Successfully logged " + ua.Username + " out.\n\n");
                        }
                        else // User is unable to logout
                        {
                            Console.Write(logoutRes.Message + "\n");
                        }
                        break;
                    case "4":
                        menu = false;
                        break;
                }
            }
        }
    }
}
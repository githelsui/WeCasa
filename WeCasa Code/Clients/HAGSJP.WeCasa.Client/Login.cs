using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Client
    {
    class Login
    {
        public Result CheckEmail(string email)
        {
            var validEmail = new Result();
            return validEmail;
        }
        public bool CheckPassword(string password)
        {
            var validPW = new Result();
            Console.WriteLine("Re-enter Password");
            string confirmPassword = Console.ReadLine();
            if (confirmPassword == password)
            {
                return true;
            }
            else
            {
                bool validC = false;
                while (validC == false)
                {
                    Console.WriteLine("Passwords do not match. Re-enter Password: ");
                    confirmPassword = Console.ReadLine();
                    if (confirmPassword == password)
                    {
                        validC = true;
                    }
                }
            }
            return false;
        }

        // TODO
        public void LoginUser(UserManager um)
        {
            
        }
    }
}

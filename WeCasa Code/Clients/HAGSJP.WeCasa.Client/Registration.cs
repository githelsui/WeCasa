using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Client
{
    class Registration
    {
        public string ValidateEmail()
        {
            Console.WriteLine("Enter Email Address: ");
            string email = Console.ReadLine();
            // If email does not exist in database, return email
            // Else loop until valid email is entered
            bool validE = false;
            while (validE == false)
            {
                Console.WriteLine("Invalid email provided. Retry again or contact system administrator: ");
                email = Console.ReadLine();
                // CONNECT TO DATABASE
                // CHECK FOR EMAIL
                validE = true;
            }
            return email;
            
        }
        public string ValidatePassword()
        {
            Console.WriteLine("Enter Password: ");
            string password = Console.ReadLine();
            /*if (password)
            {
                return password;
            }
            else
            {
                bool validP = false;
                while (validP == false)
                {
                    Console.WriteLine("Invalid passphrase provided. Retry again or contact system administrator: ");
                    password = Console.ReadLine();
                    validP = true;
                }
            }*/
            return password;
        }

        public string ConfirmPassword(string password)
        {
            Console.WriteLine("Re-enter Password");
            string confirmPassword = Console.ReadLine();
            if (confirmPassword == password)
            {
                return confirmPassword;
            }
            else {
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
            return confirmPassword;
        }
    }
}
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Client
{
    public class Home
    {
        public void HomePage(UserAccount userAccount)
        {
            Console.WriteLine("\nWelcome to WeCasa!");
            Menu mainMenu = new Menu();
            bool menu = true;
            while (menu != false)
            {
                Console.WriteLine("\n\nHome Page");
                Console.WriteLine("(1) Account Settings");
                Console.WriteLine("(2) Logout");
                Console.WriteLine("(3) Exit Home Page");
                switch (Console.ReadLine())
                {   
                    case "1": 
                        // Going to Account Settings Page
                        AccountSettings ac = new AccountSettings();
                        ac.AccountSettingsPage(userAccount);
                        break;
                    case "2":
                        Logout logout = new Logout();
                        var logoutRes = logout.LogoutUser(userAccount);
                        if (logoutRes.IsSuccessful)
                        {
                            Console.Write("Successfully logged " + userAccount.Username + " out.\n\n");
                            mainMenu.OpenMenu();
                        }
                        else // User is unable to logout
                        {
                            Console.Write(logoutRes.Message + "\n\n");
                        }
                        break;
                    case "3":
                        menu = false;
                        mainMenu.OpenMenu();
                        break;
                }
            }
            return;
        }
    }
}
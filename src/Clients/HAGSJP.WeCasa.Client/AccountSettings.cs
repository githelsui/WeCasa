using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

class AccountSettings {

    public void AccountSettingsPage(UserAccount userAccount) 
    {
        Menu mainMenu = new Menu();
        Home h = new Home();
        UserManager um = new UserManager();
        bool menu = true;
        while (menu != false)
        {
            Console.WriteLine("Account Settings");
            Console.WriteLine("(1) Delete Account");
            Console.WriteLine("(2) Exit Account Settings");

            switch(Console.ReadLine()) 
            {
                case "1": 
                    Console.WriteLine("Are you sure you want to delete your account?");
                    Console.WriteLine("(1) Yes");
                    Console.WriteLine("(2) No");

                    switch(Console.ReadLine())
                    {
                        case "1":
                            GenericUser gu = new GenericUser();
                            UserStatus userStatus = um.PopulateUserStatus(userAccount);
                            var deleteUserResult = gu.DeleteUser(userAccount,userStatus);
                            Console.WriteLine(deleteUserResult.Message);
                            mainMenu.OpenMenu();
                            menu = false;
                            break;
                    }
                    break;
                case "2":
                    menu = false;
                    h.HomePage(userAccount);
                    break;
            }
        }
    }
}
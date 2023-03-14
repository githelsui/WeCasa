using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

class AccountSettings
{

    public void AccountSettingsPage(UserAccount userAccount)
    {
        Menu mainMenu = new Menu();
        Home h = new Home();
        UserManager um = new UserManager();
        Authentication _auth = new Authentication();
        HashSaltSecurity hashSaltSecurity = new HashSaltSecurity();
        HAGSJP.WeCasa.Services.Implementations.PasswordHasher<UserAccount> passwordHasher = new HAGSJP.WeCasa.Services.Implementations.PasswordHasher<UserAccount>();
        bool menu = true;
        while (menu != false)
        {
            Console.WriteLine("Account Settings");
            Console.WriteLine("(1) Delete Account");
            Console.WriteLine("(2) Update Account");
            Console.WriteLine("(3) Exit Account Settings");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Are you sure you want to delete your account?");
                    Console.WriteLine("(1) Yes");
                    Console.WriteLine("(2) No");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            GenericUser gu = new GenericUser();
                            UserStatus userStatus = um.PopulateUserStatus(userAccount);
                            var deleteUserResult = gu.DeleteUser(userAccount, userStatus);
                            Console.WriteLine(deleteUserResult.Message);
                            mainMenu.OpenMenu();
                            menu = false;
                            break;
                    }
                    break;

                case "2":
                    Console.WriteLine("(1) Update First Name");
                    Console.WriteLine("(2) Update Last Name");
                    Console.WriteLine("(3) Update Username/Email");
                    Console.WriteLine("(4) Update Password");
                    Console.WriteLine("(5) Update Profile Icon");
                    Console.WriteLine("(6) Update Phone Number");
                    Console.WriteLine("(7) Exit");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.WriteLine("Enter new name:");
                            string firstName = Console.ReadLine();
                            while (um.ValidateName(firstName).IsSuccessful == false)
                            {
                                Console.WriteLine("Invalid Name. Please re-enter new name:");
                                firstName = Console.ReadLine();
                            }
                            um.UpdateFirstName(userAccount, firstName);
                            break;
                        case "2":
                            Console.WriteLine("Enter new name:");
                            string lastName = Console.ReadLine();
                            while (um.ValidateName(lastName).IsSuccessful == false)
                            {
                                Console.WriteLine("Invalid Name. Please re-enter new name:");
                                lastName = Console.ReadLine();
                            }
                            um.UpdateLastName(userAccount, lastName);
                            break;
                        case "3":
                            Console.WriteLine("Enter new username/email:");
                            string username = Console.ReadLine();
                            while (!um.ValidateEmail(username).IsSuccessful || um.IsUsernameTaken(username))
                            {
                                Console.WriteLine("Invalid Name. Please re-enter new username/email:");
                                username = Console.ReadLine();
                            }
                            username.ToLower();
                            um.UpdateUsername(userAccount, username);
                            break;
                        case "4":
                            Console.WriteLine("Enter Old Password:");
                            string oldPassword = Console.ReadLine();
                            while (oldPassword != userAccount.Password)
                            {
                                Console.WriteLine("Invalid Password. Please re-enter old password:");
                                oldPassword = Console.ReadLine();
                            }
                            Console.WriteLine("Enter New Password:");
                            string newPassword = Console.ReadLine();
                            while (um.ValidatePassword(newPassword).IsSuccessful == false)
                            {
                                Console.WriteLine("Invalid Password. Please re-enter new password:");
                                newPassword = Console.ReadLine();
                            }
                            Console.WriteLine("Re-enter new password:");
                            string confirmNewPassword = Console.ReadLine();
                            while (um.ValidatePassword(confirmNewPassword).IsSuccessful == false)
                            {
                                Console.WriteLine("New Password Does Not Match. Please re-enter new password:");
                            }
                            string salt = BitConverter.ToString(hashSaltSecurity.GenerateSalt(newPassword));
                            string encryptedPass = hashSaltSecurity.GetHashSaltCredentials(newPassword, salt);
                            um.UpdatePassword(userAccount, encryptedPass);
                            break;
                        case "5":
                            um.UpdateUserIcon(userAccount);
                            break;
                        case "6":
                            um.UpdatePhoneNumber(userAccount);
                            break;
                    }
                    break;

                case "3":
                    menu = false;
                    h.HomePage(userAccount);
                    break;
            }
        }
    }
}
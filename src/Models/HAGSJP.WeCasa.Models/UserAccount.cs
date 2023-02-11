using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    public class UserAccount
    {
        public UserAccount(){}
        public UserAccount(string email)
        {
            Username = email;
        }

        public UserAccount(string email, string password)
        {
            Username = email;
            Password = password;
        }

        public UserAccount(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = email;
            Password = password;
        }
        // System Assigned Key (SAK) -- primary key
        public int UserAccountId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}

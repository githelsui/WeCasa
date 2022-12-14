namespace HAGSJP.WeCasa.Models
{
    public class UserAccount
    {
        public UserAccount(string email)
        {
            Username = email;
        }

        public UserAccount(string email, string password)
        {
            Username = email;
            Password = password;
        }
        // System Assigned Key (SAK) -- primary key
        public int UserAccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}

namespace HAGSJP.WeCasa.Models
{
    public class User
    {
        public int Id;
        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }
        public string Username { get; set; }

        public string Email { get; set; }

    }
}

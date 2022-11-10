namespace HAGSJP.WeCasa.Models
{
    public class User
    {
        public User(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
        public string Username { get; set; }
    }
}

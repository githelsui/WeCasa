namespace HAGSJP.WeCasa.Models
{
    public class UserProfile
    {
        public UserProfile(string firstName, string lastName, string userId) {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
    }
}

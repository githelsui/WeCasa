namespace HAGSJP.WeCasa.Models
{
    public class UserProfile
    {
        public UserProfile(string firstName, string lastName, string userId, int age) {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            Age = age;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public int Age { get; set; }
    }
}

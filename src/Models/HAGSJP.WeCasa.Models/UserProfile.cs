using System.Xml.Linq;

namespace HAGSJP.WeCasa.Models
{
    public class UserProfile
    {
        public UserProfile() { }

        public UserProfile(string firstName, string lastName, string userId, int age) {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            Age = age;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public int? Image { get; set; }
        public float? ChoreProgress { get; set; }
        public List<string>? Notifications { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"UserProfile {{ FirstName={FirstName}, LastName='{LastName}, Image='{Image}' }}";
        }
    }
}

namespace HAGSJP.WeCasa.Models
{
    public class UserProfile
    {
        public UserProfile() { }
        public UserProfile(string fullName) {
            FullName = fullName;
        }
        public string FullName { get; set; }
        public string UserId { get; set; }
    }
}

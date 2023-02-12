namespace HAGSJP.WeCasa.Models.Security
{
    public class OTP
    {
        public OTP(string username, string code)
        {
            Username = username;
            CreateTime = DateTime.Now;
            ExpirationTime = DateTime.Now.AddMinutes(2).ToString();
            Code = code;
        }

        public string Username { get; set; }
        public DateTime CreateTime { get; set; }
        public string ExpirationTime { get; set; }
        public string Code { get; set; }
    }
}

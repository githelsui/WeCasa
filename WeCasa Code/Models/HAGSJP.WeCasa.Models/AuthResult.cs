using System.Net;
namespace HAGSJP.WeCasa.Models
{
    public class AuthResult : Result
    {
        public bool IsEnabled { get; set; }
        public bool IsAuth { get; set; }
        public bool ExistingAcc { get; set; }
        public bool HasValidCredentials { get; set; }
        public Object? ReturnedObject { get; set; }

        public bool HasValidOTP { get; set; }
        public bool ExpiredOTP { get; set; }

        public AuthResult() { }

        public AuthResult(bool isEnabled, bool isAuth, bool existingAcc, bool isSuccessful, HttpStatusCode errorStatus, string? message) 
        {
            this.IsEnabled = isEnabled;
            this.IsAuth = isAuth;
            this.ExistingAcc = existingAcc;
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }
    }
}


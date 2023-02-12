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
        public string Salt { get; set; }
        public bool HasValidOTP { get; set; }
        public string OTPCode { get; set; }
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

        public new bool Equals(AuthResult result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (AuthResult)result;
            return (IsEnabled == result1.IsEnabled) 
                    && (IsAuth == result1.IsAuth) 
                    && (ExistingAcc == result1.ExistingAcc)
                    && (IsSuccessful == result1.IsSuccessful) 
                    && (ErrorStatus == result1.ErrorStatus)
                    && (Message == result1.Message);
        }
        public static bool operator !=(AuthResult result1, AuthResult result2)        
        {
            return !result1.Equals(result2);
        }

        public static bool operator ==(AuthResult result1, AuthResult result2)        
        {
            return result1.Equals(result2);
        }
    }
}


using System.Net;
namespace HAGSJP.WeCasa.Models
{
    public class Result
    { 
        public bool IsSuccessful { get; set; }
        public HttpStatusCode ErrorStatus { get; set; }

        public string? Message { get; set; }

        public Result() { }

        public Result(bool isSuccessful, HttpStatusCode errorStatus, string? message) 
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }

        public new bool Equals(AuthResult result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (AuthResult)result;
            return (IsSuccessful == result1.IsSuccessful) 
                    && (Message == result1.Message)
                    && (ErrorStatus == result1.ErrorStatus);
        }
    }
}


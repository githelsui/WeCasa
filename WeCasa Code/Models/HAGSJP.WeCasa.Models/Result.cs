using System.Net;
namespace HAGSJP.WeCasa.Models
{
    public class Result
    { 
        public bool IsSuccessful { get; set; }
        public HttpStatusCode ErrorStatus { get; set; }

        public string? Message { get; set; }

        public Result(bool isSuccessful, HttpStatusCode errorStatus, string? message) 
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }
    }
}


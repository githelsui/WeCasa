using System.Net;
namespace HAGSJP.WeCasa.Models
{
    public class S3Result : Result
    {
        public Object? ReturnedObject { get; set; }

        public List<S3ObjectModel> Files { get; set; }

        public S3Result() { }

        public S3Result(bool isSuccessful, HttpStatusCode errorStatus, string? message) 
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }

        public new bool Equals(S3Result result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (S3Result)result;
            return ((ErrorStatus == result1.ErrorStatus)
                    && (Message == result1.Message));
        }
        public static bool operator !=(S3Result result1, S3Result result2)        
        {
            return !result1.Equals(result2);
        }

        public static bool operator ==(S3Result result1, S3Result result2)        
        {
            return result1.Equals(result2);
        }
    }
}


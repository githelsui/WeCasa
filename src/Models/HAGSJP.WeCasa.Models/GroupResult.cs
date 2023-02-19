using System.Net;
using System.Runtime.CompilerServices;

namespace HAGSJP.WeCasa.Models
{
    public class GroupResult : Result
    {
        public Object? ReturnedObject { get; set; }

        public GroupResult() { }

        public GroupResult(HttpStatusCode errorStatus, string? message)
        { 
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }

        public new bool Equals(GroupResult result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (GroupResult)result;
            return (ErrorStatus == result1.ErrorStatus)
                    && (Message == result1.Message);
        }
        public static bool operator !=(GroupResult result1, GroupResult result2)        
        {
            return !result1.Equals(result2);
        }

        public static bool operator ==(GroupResult result1, GroupResult result2)        
        {
            return result1.Equals(result2);
        }
    }
}


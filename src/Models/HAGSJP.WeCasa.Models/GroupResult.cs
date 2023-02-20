using System.Net;
using System.Runtime.CompilerServices;

namespace HAGSJP.WeCasa.Models
{
    public class GroupResult : Result
    {
        public Object? ReturnedObject { get; set; }
        public List<GroupModel> Groups { get; set; }

        public GroupResult() { }

        public GroupResult(List<GroupModel> groups, bool isSuccessful, HttpStatusCode errorStatus, string? message)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
            this.Groups = groups;
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


using System;
using System.Net;

namespace HAGSJP.WeCasa.Models
{
	public class ChoreResult : Result
	{
        public Object? ReturnedObject { get; set; }

        public ChoreResult() {}

        public ChoreResult(bool isSuccessful, HttpStatusCode errorStatus, string? message)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }

        public new bool Equals(ChoreResult result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (ChoreResult)result;
            return ((ErrorStatus == result1.ErrorStatus)
                    && (Message == result1.Message));
        }
        public static bool operator !=(ChoreResult result1, ChoreResult result2)
        {
            return !result1.Equals(result2);
        }

        public static bool operator ==(ChoreResult result1, ChoreResult result2)
        {
            return result1.Equals(result2);
        }
    }
}


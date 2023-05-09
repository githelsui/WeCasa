using System;
using System.Net;

namespace HAGSJP.WeCasa.Models
{
    public class KPIResult : Result
    {
        public Object? ReturnedObject { get; set; }

        public KPIResult() { }

        public KPIResult(bool isSuccessful, HttpStatusCode errorStatus, string? message)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }

        public new bool Equals(KPIResult result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (KPIResult)result;
            return ((ErrorStatus == result1.ErrorStatus)
                    && (Message == result1.Message));
        }
        public static bool operator !=(KPIResult result1, KPIResult result2)
        {
            return !result1.Equals(result2);
        }

        public static bool operator ==(KPIResult result1, KPIResult result2)
        {
            return result1.Equals(result2);
        }
    }
}


using System;
using System.Net;

namespace HAGSJP.WeCasa.Models
{
    public class GroceryResult : Result
    {
        public Object? ReturnedObject { get; set; }

        public GroceryResult() { }

        public GroceryResult(bool isSuccessful, HttpStatusCode errorStatus, string? message)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }

        public new bool Equals(GroceryResult result)
        {
            if (GetType() != result.GetType())
                return false;
            var result1 = (GroceryResult)result;
            return ((ErrorStatus == result1.ErrorStatus)
                    && (Message == result1.Message));
        }
        public static bool operator !=(GroceryResult result1, GroceryResult result2)
        {
            return !result1.Equals(result2);
        }

        public static bool operator ==(GroceryResult result1, GroceryResult result2)
        {
            return result1.Equals(result2);
        }
    }
}
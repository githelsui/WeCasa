using System;
using System.Net;

namespace HAGSJP.WeCasa.Models
{
	public class ResultObj
	{
        public Object ReturnedObject { get; set; }
        public bool IsSuccessful { get; set; }
        public HttpStatusCode ErrorStatus { get; set; }
        public string? Message { get; set; }

        public ResultObj() { }

        public ResultObj(bool isSuccessful, HttpStatusCode errorStatus, string? message)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
        }
    }
}


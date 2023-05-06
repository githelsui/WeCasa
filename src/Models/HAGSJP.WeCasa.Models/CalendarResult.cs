using System.Net;

namespace HAGSJP.WeCasa.Models
{
    public class CalendarResult : Result
    {
        public Object? ReturnedObject { get; set; }
        public List<Event> Events { get; set; }
        public int GroupId { get; set; }
        public CalendarResult() { }

        public CalendarResult(List<Event> events, bool isSuccessful, HttpStatusCode errorStatus, string? message)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorStatus = errorStatus;
            this.Message = message;
            this.Events = events;
        }
    }
}


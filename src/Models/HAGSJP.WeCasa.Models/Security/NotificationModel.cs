using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;


namespace HAGSJP.WeCasa.Models.Security
{
    [Serializable]
    public class NotificationModel
	{
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string ReminderOption { get; set; }
        public string EventType { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentTime { get; set; }
    }
}


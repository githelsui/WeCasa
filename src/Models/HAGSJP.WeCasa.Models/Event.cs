using System;
using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    [Serializable]
    public class Event
	{
        public Event() { }

        public Event(string eventName, string description, DateTime eventDate, int groupId, string repeats, string type, string reminder, string color, string createdBy)
        {
            EventName = eventName;
            Description = description;
            EventDate = eventDate;
            GroupId = groupId;
            Repeats = repeats;
            Type = type;
            Reminder = reminder;
            Color = color;
            CreatedBy = createdBy;
        }
        public int? EventId { get; set; }
        public string EventName { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public int GroupId { get; set; }
        public string? Repeats { get; set; }
        public string? Type { get; set; }
        public string? Reminder { get; set; }
        public string? Color { get; set; }
        public string CreatedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}

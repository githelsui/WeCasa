using System.Text.Json.Serialization;

[Serializable]
public class EventForm
{
    [JsonConstructor]
    public EventForm() { }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventTime { get; set; }
    public int? GroupId { get; set; }
    public string Repeats { get; set; }
    public string Type { get; set; }
    public string Reminder { get; set; }
    public string Notification { get; set; }
    public string CreatedBy { get; set; }
}

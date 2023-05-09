using System.Text.Json.Serialization;

[Serializable]
public class NudgeForm
{
    [JsonConstructor]
    public NudgeForm() { }
    public int NudgeId { get; set; }
    public int GroupId { get; set; }
    public int ChoreId { get; set; }
    public string SenderUsername { get; set; }
    public string ReceiverUsername { get; set; }
    public string Message { get; set; }
    //public DateTime SentAt { get; set; }
    public Boolean IsComplete { get; set; }

}
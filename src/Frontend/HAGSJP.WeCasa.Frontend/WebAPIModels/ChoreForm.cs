using System.Text.Json.Serialization;

[Serializable]
public class ChoreForm
{
    [JsonConstructor]
    public ChoreForm() { }
    public string? CurrentUser { get; set; }
    public string? Name { get; set; }
    public DateTime? ResetTime { get; set; }
    public string? Notes { get; set; }
    public int GroupId { get; set; }
    public int? ChoreId { get; set; }
    public Boolean? IsCompleted { get; set; }
    public string? Repeats { get; set; }
    public List<string>? Days { get; set; }
    public List<string>? AssignedTo { get; set; }
    public string? CurrentDate { get; set; }
}


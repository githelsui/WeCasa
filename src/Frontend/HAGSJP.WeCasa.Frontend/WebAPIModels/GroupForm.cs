using System.Text.Json.Serialization;

[Serializable]
public class GroupForm
{
    [JsonConstructor]
    public GroupForm() { }

    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public string Owner { get; set; }
    public string Icon { get; set; }
    public decimal? Budget { get; set; }
    public List<string> Features { get; set; }
}

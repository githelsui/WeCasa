using System.Text.Json.Serialization;

[Serializable]
public class GroupMemberForm
{
    [JsonConstructor]
    public GroupMemberForm() { }

    public int GroupId { get; set; }
    public List<string> GroupMembers { get; set; }
}

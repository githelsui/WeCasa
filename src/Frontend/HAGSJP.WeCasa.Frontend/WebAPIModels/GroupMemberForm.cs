using System.Text.Json.Serialization;

[Serializable]
public class GroupMemberForm
{
    [JsonConstructor]
    public GroupMemberForm() { }

    public int GroupId { get; set; }
    public string? GroupMember { get; set; }
    public string[]? GroupMembers { get; set; }
}

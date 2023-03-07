using System.Text.Json.Serialization;

[Serializable]
public class AccountForm
{
    [JsonConstructor]
    public AccountForm() { }

    public string Email { get; set; }
    public string? Password { get; set; }

    public string? NewField { get; set; }
    public List<string>? Notifications { get; set; }
}

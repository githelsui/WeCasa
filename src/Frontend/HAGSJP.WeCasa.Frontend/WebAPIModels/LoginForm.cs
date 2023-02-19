using System.Text.Json.Serialization;

[Serializable]
public class LoginForm
{
    [JsonConstructor]
    public LoginForm() { }

    public string Username { get; set; }
    public string? Password { get; set; }
}
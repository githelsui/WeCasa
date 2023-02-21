using System.Text.Json.Serialization;

[Serializable]
public class RegistrationForm
{
    [JsonConstructor]
    public RegistrationForm() { }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
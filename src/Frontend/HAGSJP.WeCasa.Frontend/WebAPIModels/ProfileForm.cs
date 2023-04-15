using System.Text.Json.Serialization;

[Serializable]
public class ProfileForm
{
    [JsonConstructor]
    public ProfileForm() { }

    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Image { get; set; }
}

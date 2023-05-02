using System.Text.Json.Serialization;

[Serializable]
public class FileForm
{
    [JsonConstructor]
    public FileForm() { }
    public string? FileName { get; set; }
    public IFormFile? File { get; set; } 
    public string? Owner { get; set; }
    public string? GroupId { get; set; }
}

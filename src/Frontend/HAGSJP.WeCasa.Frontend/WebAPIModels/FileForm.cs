using System.Text.Json.Serialization;

[Serializable]
public class FileForm
{
    [JsonConstructor]
    public FileForm() { }
    public string FileName { get; set; }

}

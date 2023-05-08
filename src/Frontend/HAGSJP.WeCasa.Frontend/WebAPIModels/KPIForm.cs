using System.Text.Json.Serialization;

[Serializable]
public class KPIForm
{
    [JsonConstructor]
    public KPIForm() { }

    public String TimeFrame { get; set; }
    public String CurrentUser { get; set; }
}

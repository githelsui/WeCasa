using System.Text.Json.Serialization;

[Serializable]
public class GroceryForm
{
    [JsonConstructor]
    public GroceryForm() { }
    public string? CurrentUser { get; set; }
    public string Name { get; set; }
    public string? Notes { get; set; }
    public int GroupId { get; set; }
    public int? GroceryId { get; set; }
    public Boolean? IsPurchased { get; set; }
    public List<string>? Assignments { get; set; }
}
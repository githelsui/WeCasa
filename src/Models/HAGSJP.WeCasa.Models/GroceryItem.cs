using System;
using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    [Serializable]
    public class GroceryItem
    {
        public int? GroceryId { get; set; }
        public String Name { get; set; }
        public String? Notes { get; set; }
        public int GroupId { get; set; }
        public Boolean? IsPurchased { get; set; }
        public List<String>? Assignments { get; set; } //day string

        // Assigned in Manager layer 
        public DateTime? Created { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public String? LastUpdatedBy { get; set; }


        [JsonConstructor]
        public GroceryItem() { }
        public GroceryItem(int groupId, String name, String? notes, List<string> assignments)
        {
            Name = name;
            Notes = notes;
            GroupId = groupId;
            Assignments = assignments;
        }
    }
}
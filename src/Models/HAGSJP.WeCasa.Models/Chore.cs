using System;
using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    [Serializable]
    public class Chore
	{
        public int? ChoreId { get; set; }
        public String Name { get; set; }
        public DateTime? ResetTime { get; set; } // Chore due date
        public String? Notes { get; set; }
        public String? Assignment { get; set; } //TODO: Remove property in model 
        public int GroupId { get; set; }
        public Boolean? IsRepeated { get; set; } //TODO: Remove property in model
        public Boolean? IsCompleted { get; set; }

        public DateTime? Created { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public String? LastUpdatedBy { get; set; }
        public List<string> AssignedTo { get; set; }
        public String? Repeats { get; set; }

        [JsonConstructor]
        public Chore(){}
	}
}


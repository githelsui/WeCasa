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
        public int GroupId { get; set; }
        public Boolean? IsCompleted { get; set; }
        public List<string> AssignedTo { get; set; }
        public String? Repeats { get; set; }

        //Assigned in Manager layer
        public DateTime? Created { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public String? LastUpdatedBy { get; set; }
        

        [JsonConstructor]
        public Chore(){}
	}
}


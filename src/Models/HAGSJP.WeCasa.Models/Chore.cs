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
        public String? Repeats { get; set; }
        public List<UserProfile> AssignedTo { get; set; }
        public List<String> UsernamesAssignedTo { get; set; }
        public List<int> UserChoreIds { get; set; } // List of userchore_id's per each assigned task and respective user

        //Assigned in Manager layer
        public DateTime? Created { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public String? LastUpdatedBy { get; set; }
        

        [JsonConstructor]
        public Chore(){}
        public Chore(String name)
        {

        }
    }
}


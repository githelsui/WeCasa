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
        public List<UserProfile>? AssignedTo { get; set; }
        public List<String>? UsernamesAssignedTo { get; set; }
        public List<String>? Days { get; set; } //day string

        // Assigned in Manager layer 
        public DateTime? Created { get; set; }  
        public String? CreatedBy { get; set; } 
        public DateTime? LastUpdated { get; set; }
        public String? LastUpdatedBy { get; set; }
        

        [JsonConstructor]
        public Chore(){}

        public Chore(String name, int groupId, string createdBy)
        {
            Name = name;
            GroupId = groupId;
            CreatedBy = createdBy;
        }

        public Chore(String name, List<String>? days, String? notes, int groupId, List<string> usernamesAssignedTo, String? repeats)
        {
            Name = name;
            Days = days;
            Notes =notes;
            GroupId = groupId;
            UsernamesAssignedTo = usernamesAssignedTo;
            Repeats = repeats;
        }
        public Chore(int? choreId, String name, List<String>? days, String? notes, int groupId, List<string> usernamesAssignedTo, String? repeats)
        {
            ChoreId = choreId;
            Name = name;
            Days = days;
            Notes = notes;
            GroupId = groupId;
            UsernamesAssignedTo = usernamesAssignedTo;
            Repeats = repeats;
        }
    }
}


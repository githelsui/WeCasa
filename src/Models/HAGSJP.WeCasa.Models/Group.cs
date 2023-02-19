using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    public class GroupModel
    {
        public GroupModel(){}
        public GroupModel(int id, string owner, string name, string icon)
        {
            GroupId = id;
            Owner = owner;
            GroupName = name;
            Icon = icon;
        }
        public GroupModel(int id, string owner, string name, string icon, decimal budget)
        {
            GroupId = id;
            Owner = owner;
            GroupName = name;
            Icon = icon;
            Budget = budget;
        }
  
        // System Assigned Key (SAK) -- primary key
        public int GroupId { get; set; }
        public string Owner { get; set; }
        public string GroupName { get; set; }
        public string Icon { get; set; }
        public decimal Budget { get; set; }

        public List<string>? Features { get; set; }
        
        public List<string>? Users { get; set; }
    }
}

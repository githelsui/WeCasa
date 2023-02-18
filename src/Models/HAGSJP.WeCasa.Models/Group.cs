using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    public class Group
    {
        public Group(){}
        public Group(int id, string owner, string name, string icon)
        {
            GroupId = id;
            Owner = owner;
            GroupName = name;
            Icon = icon;
        }
        public Group(int id, string owner, string name, string icon, float budget, List<string> features)
        {
            GroupId = id;
            Owner = owner;
            GroupName = name;
            Icon = icon;
            Budget = budget;
            Features = features;
        }

        // System Assigned Key (SAK) -- primary key
        public int GroupId { get; set; }
        public string Owner { get; set; }
        public string GroupName { get; set; }
        public string Icon { get; set; }
        public float? Budget { get; set; }

        public List<string>? Features { get; set; }
    }
}

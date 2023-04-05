using System;
using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    [Serializable]
    public class Chore
	{
        public int? ChoreId { get; set; }
        public String Name { get; set; }
        public DateTime? ResetTime { get; set; }
        public String? Notes { get; set; }
        public String? Assignment { get; set; }
        public int GroupId { get; set; }
        public Boolean? IsRepeated { get; set; }
        public Boolean? IsCompleted { get; set; }

        [JsonConstructor]
        public Chore(){}
	}
}


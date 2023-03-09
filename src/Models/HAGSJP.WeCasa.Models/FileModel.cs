using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace HAGSJP.WeCasa.Models
{
    public class FileModel
    {
        public FileModel(){}

        public FileModel(string objectKey)
        {
            ObjectKey = objectKey;
        }
        public FileModel(string objectKey, int groupid, string fileName, string type, string owner, string lastUpdated)
        {
            ObjectKey = objectKey;
            GroupId = groupid;
            Owner = owner;
            Type = type;
            LastUpdated = lastUpdated;
        }

        public string ObjectKey { get; set; }
        public string? Owner { get; set; }
        public string? Type { get; set; }
        public decimal? Size { get; set; }
        public int? GroupId { get; set; }
        public string? LastUpdated { get; set; }
    }
}

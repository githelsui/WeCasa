using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace HAGSJP.WeCasa.Models
{
    public class S3ObjectModel
    {
        public S3ObjectModel(){}

        public S3ObjectModel(string fileName)
        {
            FileName = fileName;
        }
        public S3ObjectModel(string fileName, string data, string size, DateTime lastUpdated)
        {
            FileName = fileName;
            Data = data;
            Size = size;
            LastUpdated = lastUpdated;
        }

        public string FileName { get; set; }
        public string Data { get; set; }
        public string? PresignedUrl { get; set; }
        public string? Owner { get; set; }
        public string? ContentType { get; set; }
        public string? Size { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}

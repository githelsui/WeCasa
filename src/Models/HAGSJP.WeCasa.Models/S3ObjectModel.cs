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
        public S3ObjectModel(string fileName, string size, DateTime lastUpdated)
        {
            FileName = fileName;
            Size = size;
            LastUpdated = lastUpdated;
        }
        public S3ObjectModel(string fileName, byte[] data, string size, DateTime lastUpdated)
        {
            FileName = fileName;
            Data = data;
            Size = size;
            LastUpdated = lastUpdated;
        }

        public S3ObjectModel(string fileName, byte[] data, string type, string size, DateTime lastUpdated)
        {
            FileName = fileName;
            Data = data;
            ContentType = type;
            Size = size;
            LastUpdated = lastUpdated;
        }

        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string? Url { get; set; }
        public string? Owner { get; set; }
        public string? ContentType { get; set; }
        public string? Size { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}

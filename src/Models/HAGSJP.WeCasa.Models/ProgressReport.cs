using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace HAGSJP.WeCasa.Models
{
    public class ProgressReport
    {
        public ProgressReport(){}
        public ProgressReport(int id, string username)
        {
            GroupId = id;
            Username = username;    
        }
        public ProgressReport(int completedChores, int incompleteChores)
        {
            CompletedChores = completedChores;
            IncompleteChores = incompleteChores;
        }
        
        public string Username { get; set; }
        public int GroupId { get; set; }
        public int CompletedChores { get; set; }
        public int IncompleteChores { get; set; }
    }
}

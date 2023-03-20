using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    public class BudgetBarUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public decimal TotalSpent { get; set; }

        public BudgetBarUser(string username, string firstName)
        {
            Username = username;
            FirstName = firstName;
        }
    }

    public class BudgetBarView 
    {
        public List<Bill> ActiveBills { get; set; }
        public List<Bill> DeletedBills { get; set; }
        public List<BudgetBarUser> Group { get; set; }
        public decimal Budget { get; set; }
        public decimal GroupTotal { get; set; }
        public Dictionary<string, decimal> UserTotals { get; set; }
    }
}


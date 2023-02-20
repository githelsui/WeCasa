using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    public class BudgetBarUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public decimal TotalSpent { get; set; }
        public List<Bill> ActiveBills { get; set; }
        public List<Bill> DeletedBills { get; set; }

        public BudgetBarUser()
        {
            ActiveBills = new List<Bill>();
            DeletedBills = new List<Bill>();
        }

        public BudgetBarUser(string username, string firstName)
        {
            Username = username;
            FirstName = firstName;
        }
    }
}

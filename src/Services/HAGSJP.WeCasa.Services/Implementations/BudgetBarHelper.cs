using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class BudgetBarHelper
    {
        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public decimal CalculateTotal(List<Bill> bills) 
        {
            decimal total = 0;
            foreach (Bill bill in bills)
            {
                total += bill.Amount;
            }
            return total;
        }

        public Dictionary<string, List<Bill>> SortBillsByUsername(List<Bill> bills, List<string> usernames)
        {
            Dictionary<string, List<Bill>> sortedBills = new Dictionary<string,  List<Bill>>();
            foreach(Bill bill in bills)
            {
                if (sortedBills[bill.Username] == null)
                {
                    sortedBills.Add(bill.Username, new List<Bill>());
                }
                sortedBills[bill.Username].Add(bill);
            }
            return sortedBills;
        }
    }
}
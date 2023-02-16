using System.Text.Json.Serialization;
using HAGSJP.WeCasa.Models;

// [Serializable]
// public class BudgetBarViewRequest 
// {
//     [JsonConstructor]
//     public BudgetBarViewRequest(){}
//     public string Username { get; set; }
//     public string GroupId { get; set; }
// }

[Serializable]
public class UserTableViewResponse 
{
    [JsonConstructor]
    public UserTableViewResponse() {}
    public string Username { get; set; }
    public decimal Total { get; set; }
    public List<Bill> ActiveBills { get; set; }
    public List<Bill> DeletedBills { get; set; }
    }


[Serializable]
public class InitialBudgetBarViewResponse 
{
    [JsonConstructor]
    public InitialBudgetBarViewResponse() {}
    public List <string> Group { get; set; }
    public decimal Budget { get; set; }
    public decimal GroupTotal { get; set; }
    public Dictionary<string, decimal> TotalSpentPerMember { get; set; } 
}

// [Serializable]
// public class BudgetBarViewResponse 
// {
//     [JsonConstructor]
//     public BudgetBarViewResponse() {}
//     public List <string> Group { get; set; }
//     public decimal Budget { get; set; }
//     public decimal TotalSpent { get; set; }
//     public Dictionary<string, decimal> TotalSpentPerMember { get; set; } 
//     public Dictionary<string, List<Bill>> ActiveBills { get; set; }
//     public Dictionary<string, List<Bill>> DeletedBills { get; set; }
// }


[Serializable]
public class AddBillRequest
{
    [JsonConstructor]
    public AddBillRequest() {}
    public List <string> Usernames { get; set; }
    public Bill Bill { get; set; }
}

[Serializable]
public class UpdateBudgetRequest
{
    [JsonConstructor]
    public UpdateBudgetRequest() {}
    public string GroupId { get; set; }
    public decimal Amount { get; set; }
}
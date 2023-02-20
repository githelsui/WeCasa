using System.Text.Json.Serialization;
using HAGSJP.WeCasa.Models.Security;


namespace HAGSJP.WeCasa.Models
{
     [Serializable]
   public class Bill
   {
       public string Username { get; set; }
       public int BillId { get; set; }
       public int GroupId { get; set; }
       public DateTime? DateEntered { get; set; }
       public string BillName { get; set; }
       public string? BillDescription { get; set; }
       public decimal Amount { get; set; }
       public Boolean? PaymentStatus { get; set; }
       public Boolean? IsRepeated { get; set; }
       public Boolean? IsDeleted { get; set; }
       public DateTime? DateDeleted { get; set; }
       public string? PhotoFileName { get; set; }
       
     [JsonConstructor]
       public Bill(){}

        public Bill(DateTime dateEntered, string billName , string billDescription, decimal amount, Boolean paymentStatus,Boolean isRepeated, Boolean isDeleted, DateTime dateDeleted, string photoFileName)
       {
            Username = "";
            DateEntered = dateEntered;
            BillName = billName;
            BillDescription = billDescription;
            Amount = amount;
            PaymentStatus = paymentStatus;
            IsRepeated = isRepeated;
            IsDeleted = isDeleted;
            DateDeleted = dateDeleted;
            PhotoFileName = photoFileName;
       }

       public Bill(string username, int billId, DateTime dateEntered, string billName , string billDescription, decimal amount, Boolean paymentStatus,Boolean isRepeated, Boolean isDeleted, DateTime dateDeleted, string photoFileName)
       {
            Username = username;
            BillId = billId;
            DateEntered = dateEntered;
            BillName = billName;
            BillDescription = billDescription;
            Amount = amount;
            PaymentStatus = paymentStatus;
            IsRepeated = isRepeated;
            IsDeleted = isDeleted;
            DateDeleted = dateDeleted;
            PhotoFileName = photoFileName;
       }
   }
}

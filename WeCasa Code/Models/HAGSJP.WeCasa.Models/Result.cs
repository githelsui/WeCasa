namespace HAGSJP.WeCasa.Models
{
    public class Result
    { 
        public bool IsSuccessful { get; set; }

        public string? ErrorMessage { get; set; }

        public object Payload;
    }
}

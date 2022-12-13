using System;
namespace HAGSJP.WeCasa.Models
{
    public class UserOperation
    {
        public UserOperation(Operations op, int success)
        {
            Operation = op;
            Success = success;
        }

        public Operations Operation { get; set; }
        public int Success { get; set; }
    }
}


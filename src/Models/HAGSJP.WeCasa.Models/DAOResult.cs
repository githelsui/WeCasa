namespace HAGSJP.WeCasa.Models
{
    public class DAOResult: Result
    { 
        public string? ErrorStatus;
         public string? SqlState;

         public object? ReturnedObject;

        public DAOResult ValidateSqlResult(int rows)
        {
            var result = new DAOResult();
            if (rows == 1)
            {
                result.IsSuccessful = true;
                result.Message = "1 entry affected";
                return result;
            }
            result.IsSuccessful = false;
            result.Message = $"The number of rows affected were {rows}";
            return result;
        }
    }
}
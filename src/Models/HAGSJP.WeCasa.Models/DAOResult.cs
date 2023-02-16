namespace HAGSJP.WeCasa.Models
{
    public class DAOResult: Result
    { 
        public string? ErrorStatus;
         public string? SqlState;

        public DAOResult ValidateSqlResult(int rows)
        {
            var result = new DAOResult();
            if (rows == 1)
            {
                result.IsSuccessful = true;
                result.Message = string.Empty;
                return result;
            }
            result.IsSuccessful = false;
            result.Message = $"Rows affected were not 1. It was {rows}";
            return result;
        }

         public DAOResult ValidateSqlResultMultiple(int rows)
        {
            var result = new DAOResult();
            if (rows >= 0)
            {
                result.IsSuccessful = true;
                result.Message = $"The number of rows affected were {rows}";
                return result;
            }
            result.IsSuccessful = false;
            result.Message = "No entries were affected";
            return result;
        }
    }
}
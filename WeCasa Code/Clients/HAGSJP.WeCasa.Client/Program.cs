using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingDAO l = new LoggingDAO();
            Console.WriteLine("Hello, World!");
            string input = Console.ReadLine();
            l.LogData(input);
        }
    }
}
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MariaDBLoggingDAO l = new MariaDBLoggingDAO();
            Console.WriteLine("Hello, World!");
            string input = Console.ReadLine();
            l.LogData(input);
        }
    }
}
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Client
{
    public class Home
    {
        public void HomePage()
        {
            Console.WriteLine("Welcome to WeCasa!");
            Console.WriteLine("Type x to exit:");
            string input = "";
            while (!input.Equals("x")) {
                input = Console.ReadLine();
            }
            return;
        }
    }
}
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Console menu
            Menu menu = new Menu();
            menu.OpenMenu();
        }
    }
}
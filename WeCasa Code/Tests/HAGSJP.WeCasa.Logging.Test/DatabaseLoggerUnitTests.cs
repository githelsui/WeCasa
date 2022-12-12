using System.Threading.Tasks;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Logging.Test
{
    [TestClass]
    public class DatabaseLoggerUnitTest
    {
        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
           var expected = typeof(Logger);

           var actual = new Logger(new AccountMariaDAO());

           Assert.IsNotNull(actual);
           Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public async Task ShouldLogSuccessfully()
        {
             var expected = new Result();
             expected.IsSuccessful = true;

            AccountMariaDAO testMariaDao = new AccountMariaDAO();
            Logger testLogger = new Logger(testMariaDao);
            var actual = await testLogger.Log("test Log Message", LogLevel.Info, "Business", "testUser");
            Console.Write("DAOOO" + actual.Message);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
        }

        [TestMethod]
        public async Task ShouldValidateCorrectCharacters()
        {
            var expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Message contains invalid character: $";

            AccountMariaDAO testMariaDao = new AccountMariaDAO();
            Logger testLogger = new Logger(testMariaDao);
            var actual = await testLogger.Log("test$", LogLevel.Info, "Business", "testUser");
            Console.Write("ERROR"+ actual.Message);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public async Task ShouldCreateInstanceWithValidLength()
        {
            //Arrange
            var expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Message log too long";

            //Act
            AccountMariaDAO testMariaDao = new AccountMariaDAO();
            Logger testLogger = new Logger(testMariaDao);
            var actual = await testLogger.Log("Ao0fQks6zVX7vylbYjfJ4Iu9u5Zd1vr014cZrIyRHSdGTzhF9aAbkGDNOpohAA0zqw3XxJqxO0wxSmJ140A3BXtpLoxvnwv2iscx7Yexy6OlKAru1mXo3tDE9OO23aIJ91k9sowYDRf9TDKPugo3qifVzOA63M5TTCGx2e89kfdNIefCRbiLjNWT1iZbh3TZz3vjwSEwP", LogLevel.Info, "Business", "testUser");

            //Assert (2 options)
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        // [TestMethod]
        // public async Task ShouldCreateInstanceWithValidLogLevelOnly()
        // {
        //     //Arrange
        //     var expected = new Result();
        //     expected.IsSuccessful = false;
        //     expected.ErrorMessage = "Invalid log level";

        //     //Act
        //     MariaDbDAO testMariaDao = new MariaDbDAO();
        //     Logger testLogger = new Logger(testMariaDao);
        //     var actual = await testLogger.Log("Testing", "Invalid Log Level", "Business", "testUser");

        //     //Assert (2 options)
        //     Assert.IsNotNull(actual);
        //     Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
        //     Assert.IsTrue(actual.ErrorMessage == expected.ErrorMessage);
        // }

        [TestMethod]
        public async Task ShouldCreateInstanceWithValidCategoryOnly()
        {
            //Arrange
            var expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Invalid category";

            //Act
            AccountMariaDAO testMariaDao = new AccountMariaDAO();
            Logger testLogger = new Logger(testMariaDao);
            var actual = await testLogger.Log("Testing", LogLevel.Info, "Invalid Category", "testUser");

            //Assert (2 options)
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }
    }
}
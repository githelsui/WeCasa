using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Frontend.Controllers;
using Amazon.S3.Model;

namespace HAGSJP.WeCasa.UserFeedback.Test
{
    [TestClass]
    public class UserFeedbackIntegrationTest
    {
        private UserFeedbackDAO userFeedbackDAO;
        private FeedbackManager feedbackManager;
        private Feedback feedback;
        [TestInitialize]
        public void Initialize()
        {
            userFeedbackDAO = new UserFeedbackDAO();
            feedbackManager = new FeedbackManager();
            feedback = new Feedback()
            {
                SubmissionDate = DateTime.Now,
                FirstName = "Joshua",
                LastName = "Quibin",
                Email = "joshuaquibin@gmail.com",
                FeedbackType = true,
                FeedbackMessage = "Great App!",
                FeedbackRating = 5,
                ResolvedStatus = false,
                ResolvedDate = null
            };
        }
        [TestMethod]
        public void FeedbackUploadTest() 
        {
            //Arrange
            var systemUnderTest = new FeedbackManager();
            //Act
            var testResult = systemUnderTest.storeFeedbackTicket(feedback);
            //Assert
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.IsSuccessful);
        }
    }
}
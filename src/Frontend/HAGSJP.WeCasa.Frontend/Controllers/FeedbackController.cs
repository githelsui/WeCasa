using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers;
using HAGSJP.WeCasa.Managers.Implementations;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    public class FeedbackController : ControllerBase
    {
        [HttpPost]
        [Route("uploadfeedback")]
        public Result UploadFeedback(Feedback feedback)
        {
            FeedbackManager fm = new FeedbackManager();
            var result = fm.storeFeedbackTicket(feedback);
            return result;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers;
using HAGSJP.WeCasa.Managers.Implementations;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackManager _feedbackManager;
        TaskCompletionSource<IActionResult> tcs;

        public FeedbackController()
        {
            _feedbackManager = new FeedbackManager();
            tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        [HttpPost]
        [Route("uploadfeedback")]
        public Task<IActionResult> Post([FromBody]Feedback feedback)
        {
            try 
            {
                Result result = _feedbackManager.storeFeedbackTicket(feedback);
                if (!result.IsSuccessful)
                {
                    tcs.SetResult(BadRequest());
                    return tcs.Task;
                }
                tcs.SetResult(Ok(true));
                return tcs.Task;
            }
            catch(Exception ex)
            {
                tcs.SetResult(StatusCode(500));
                return tcs.Task;
            }
        }
    }
}

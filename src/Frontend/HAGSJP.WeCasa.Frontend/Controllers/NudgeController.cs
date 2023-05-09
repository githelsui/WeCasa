using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using HAGSJP.WeCasa.Services.Implementations;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;



namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("nudge")]
    public class NudgeController : ControllerBase
    {
        private readonly Logger _logger;
        private readonly NudgeManager _nudgeManager;
        private readonly IConfiguration _configuration;


        public NudgeController(IConfiguration configuration)
        {
            _logger = new Logger(new AccountMariaDAO());
            _nudgeManager = new NudgeManager();
            _configuration = configuration;
        }

        private async Task SendNudgeEmail(string receiverEmail, string message)
        {
            try
            {
                Console.WriteLine("SendNudgeEmail method called.");
                //var apiKey = "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";//_configuration["SendGrid:ApiKey"];
                var apiKey = _configuration["SendGrid:ApiKey"];

                Console.WriteLine(apiKey);
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("wecasacsulbcecs@gmail.com", "WeCasa");
                var subject = "Nudge Notification";
                var to = new EmailAddress(receiverEmail, "WeCasa");
                var plainTextContent = message;
                var htmlContent = $"<p>{message}</p>";


                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                Console.WriteLine("Email message created.");

                var response = await client.SendEmailAsync(msg);
                Console.WriteLine($"Email sent with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while sending email: {ex.Message}");
            }

        }

        [HttpPost]
        // [Route("logger/LogData")]
        [Route("LogData")]
        public async Task<ActionResult<Result>> LogData([FromBody] Log log)
        {
            try
            {
                if (Enum.TryParse(log.LogLevel, out LogLevels parsedLogLevel))
                {
                    var result = await _logger.Log(log.Message, parsedLogLevel, log.Category, log.Username);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { errorMessage = "Invalid log level" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMessage = $"Internal server error: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }


        [HttpPost]
        [Route("SendNudge")]
        public async Task<ActionResult<Result>> SendNudge([FromBody] Nudge nudge)
        {
            try
            {
                var lastNudgeSent = _nudgeManager.GetLastNudgeSent(nudge.ChoreId, nudge.SenderUsername, nudge.ReceiverUsername);
                if (lastNudgeSent.HasValue && (DateTime.UtcNow - lastNudgeSent.Value).TotalHours < 24)
                {
                    return StatusCode(429, new { errorMessage = "You can only send one nudge per 24 hours" });
                }

                var result = _nudgeManager.SendNudge(nudge);
                await SendNudgeEmail(nudge.ReceiverUsername, nudge.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMessage = $"Internal server error: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }
    }
}










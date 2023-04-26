using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class SendGridService
    {
        public async Task SendFeedback()
        {
            var apiKey = "123";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("wecasacorporation@gmail.com", "WeCasa Corp");
            var subjectLine = "Test Feedback!";
            var to = new EmailAddress("joshuaquibin@gmail.com", "Joshua Quibin");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subjectLine, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new InvalidOperationException($"Failed to send email: {response.StatusCode}");
            }
        }
    }
}

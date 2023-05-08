using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Models;
using Azure;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class NotificationService
    {
        private static readonly RemindersDAO _dao = new RemindersDAO();
        private static readonly SendGridClient _sendGridClient = GetSendGridClient();

        private static SendGridClient GetSendGridClient()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .Build();

            var sendGridApiKey = config.GetValue<string>("SendGridAPIKey");

            return new SendGridClient(sendGridApiKey);
        }


        //Method to send email
        public static async Task<bool> ScheduleReminderEmail(string from, string to, string subject, string message, string reminderOption, string eventType)
        {
           

            // Create email message
            var emailFrom = new EmailAddress(from);
            var emailTo = new EmailAddress(to);
            var emailMessage = MailHelper.CreateSingleEmail(emailFrom, emailTo, subject, message, message);
            bool isSent = false;

            

            // Calculate the send date based on the selected reminder option
            DateTime sendDate;
            switch (reminderOption.ToLower())
            {
                case "immediately":
                    sendDate = DateTime.UtcNow;
                    break;
                case "Every Sunday":
                    sendDate = GetNextSunday();
                    break;
                default:
                    throw new ArgumentException("Invalid reminder option");
                    Console.WriteLine("invalid remindertime");
            }

            // Schedule the email to send at the calculated send date
            var sendAtUnixTimestamp = new DateTimeOffset(sendDate).ToUnixTimeSeconds();
            emailMessage.SendAt = sendAtUnixTimestamp;
            var _logger = new Logger(new AccountMariaDAO());
            //Logging
            try
            {
        var response = await _sendGridClient.SendEmailAsync(emailMessage);
                Console.WriteLine("Response code: " + response.StatusCode);
                // Log success
                _logger.Log("Email sent successfully", LogLevels.Info, "Data Store", to);

                isSent = true;
                return isSent;
            }
            catch (Exception ex)
            {
                // Log error
                _logger.Log("Error sending email: " + ex.Message, LogLevels.Error, "Data Store", to);
                return isSent;
            }


        }

        static DateTime GetNextSunday()
        {
            var today = DateTime.Today;
            var daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilSunday);
        }
        public static async Task<bool> SendFeedbackNotification(Feedback feedback)
        {
            var isSent = false;
            var from = new EmailAddress("wecasacsulb@gmail.com", string.Format("{0} {1}", feedback.FirstName, feedback.LastName));
            var subject = "New WeCasa Feedback Submission";
            var to = new EmailAddress("wecasacsulb@gmail.com", "WeCasa CSULB");
            var plainTextContent = string.Format("Feedback Message: {0}", feedback.FeedbackMessage);
            var htmlContent = string.Format("Feedback Message: {0}", feedback.FeedbackMessage);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var _logger = new Logger(new AccountMariaDAO());
            try
            {
                var response = await _sendGridClient.SendEmailAsync(msg);
                Console.WriteLine("Response: " + response.StatusCode);
                // Log success
                _logger.Log("Email sent successfully", LogLevels.Info, "Data Store", feedback.Email);
                isSent = true;
                return isSent;
            }
            catch (Exception ex)
            {
                // Log error
                _logger.Log("Error sending email: " + ex.Message, LogLevels.Error, "Data Store", feedback.Email);
                Console.WriteLine(isSent);
                return isSent;
            }
        }
    }
}




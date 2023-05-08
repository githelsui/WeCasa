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
using System.Threading.Tasks;
using HAGSJP.WeCasa.Models;
using Azure;
using System.Net;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class NotificationService
    {
        private readonly RemindersDAO _dao;

        public NotificationService()
        {
            _dao = new RemindersDAO();
        }

        //Method to send email
        public static async Task<bool> ScheduleReminderEmail(string from, string to, string subject, string message, string reminderOption, string eventType)
        {
            var apiKey = "";
            var client = new SendGridClient(apiKey);

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
                var response = await client.SendEmailAsync(emailMessage);
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


    }
}




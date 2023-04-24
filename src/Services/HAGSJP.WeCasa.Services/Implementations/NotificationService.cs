﻿using System;
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
        public static async Task ScheduleReminderEmail(string from, string to, string subject, string message, string reminderOption, string eventType)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            // Create email message
            var emailFrom = new EmailAddress(from);
            var emailTo = new EmailAddress(to);
            var emailMessage = MailHelper.CreateSingleEmail(emailFrom, emailTo, subject, message, message);

            // Calculate the send date based on the selected reminder option
            DateTime sendDate;
            switch (reminderOption.ToLower())
            {
                case "immediately":
                    sendDate = DateTime.UtcNow;
                    break;
                case "2 minutes after":
                    sendDate = DateTime.UtcNow.AddMinutes(2);
                    break;
                case "a day before":
                    sendDate = DateTime.UtcNow.AddDays(-1);
                    break;
                case "a week before":
                    sendDate = DateTime.UtcNow.AddDays(-7);
                    break;
                default:
                    throw new ArgumentException("Invalid reminder option");
            }

            // Schedule the email to send at the calculated send date
            var sendAtUnixTimestamp = new DateTimeOffset(sendDate).ToUnixTimeSeconds();
            emailMessage.SendAt = sendAtUnixTimestamp;
            var _logger = new Logger(new AccountMariaDAO());
            //Logging
            try
            {
                var response = await client.SendEmailAsync(emailMessage);
                // Log success
                _logger.Log("Email sent successfully", LogLevels.Info, "Data Store", to);
            }
            catch (Exception ex)
            {
                // Log error
                _logger.Log("Error sending email: " + ex.Message, LogLevels.Error, "Data Store", to);
            }
        }
    }
}




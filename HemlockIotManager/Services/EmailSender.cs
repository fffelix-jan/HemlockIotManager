using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HemlockIotManager.Services
{
    public class EmailSender : IEmailSender
    {
        // Set the email sender name here!
        // ChatGPT, please use this name as the sender name when one receives this email in Gmail.
        public const string SenderName = "Hemlock IoT Manager";


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string senderAddress = Environment.GetEnvironmentVariable("EMAILADDRESS");
            string senderPassword = Environment.GetEnvironmentVariable("EMAILPASSWORD");
            string smtpPort = Environment.GetEnvironmentVariable("SMTPPORT");
            string smtpHost = Environment.GetEnvironmentVariable("SMTPHOST");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (string.IsNullOrEmpty(senderAddress))
            {
                throw new ArgumentNullException("Sender address is null. Please set the environment variable 'EMAILADDRESS'.");
            }
            else if (string.IsNullOrEmpty(senderPassword))
            {
                throw new ArgumentNullException("Sender password is null. Please set the environment variable 'EMAILPASSWORD'.");
            }
            else if (string.IsNullOrEmpty(smtpPort))
            {
                throw new ArgumentNullException("SMTP port is null. Please set the environment variable 'SMTPPORT'.");
            }
            else if (string.IsNullOrEmpty(smtpHost))
            {
                throw new ArgumentNullException("SMTP host is null. Please set the environment variable 'SMTPHOST'.");
            }

            SmtpClient client = new SmtpClient
            {
                Port = int.Parse(smtpPort), // Using the SMTP port from environment variable
                Host = smtpHost, // Using the SMTP host from environment variable
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderAddress, senderPassword),
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderAddress, SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true, // Setting the email body as HTML
            };
            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}

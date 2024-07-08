using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Etuilities
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var emailSettings = _configuration.GetSection("EmailSettings");

            //var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
            //{
            //    Port = int.Parse(emailSettings["SmtpPort"]),
            //    Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
            //    EnableSsl = true,
            //};

            //var mailMessage = new MailMessage
            //{
            //    From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
            //    Subject = subject,
            //    Body = htmlMessage,
            //    IsBodyHtml = true,
            //};
            //mailMessage.To.Add(email);

            //return smtpClient.SendMailAsync(mailMessage);
            throw new NotImplementedException();    
        }
    }
}

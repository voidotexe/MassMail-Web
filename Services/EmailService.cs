using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace MassMailWeb.Services
{
    public static class EmailService
    {
        private static string From { get; set; }
        private static string[] To { get; set; }
        private static MimeMessage Message { get; set; } = new MimeMessage();
        private static string SmtpServer { get; set; }
        private static int SmtpServerIndex { get; set; }

        public static MimeMessage SetMessage(string from, string toField, string subject, string body, List<string> attachments, bool bccOrNot, bool HtmlOrNot)
        {
            From = from;

            Message.From.Add(MailboxAddress.Parse(From));

            To = toField.Split(",");

            foreach (string to in To)
            {
                if (bccOrNot)
                {
                    Message.Bcc.Add(MailboxAddress.Parse(to));
                }
                else
                {
                    Message.To.Add(MailboxAddress.Parse(to));
                }
            }

            Message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();

            foreach (string attachment in attachments)
            {
                bodyBuilder.Attachments.Add(attachment);
            }

            if (!HtmlOrNot)
            {
                bodyBuilder.TextBody = body + "\n\nEnviado pelo MassMail (Web): https://github.com/voidotexe/MassMail-Web";
            }
            else
            {
                bodyBuilder.HtmlBody = body + "<br /><br /><a href='https://github.com/voidotexe/MassMail-Web'>Enviado pelo MassMail (Web)</a>";
            }

            Message.Body = bodyBuilder.ToMessageBody();

            return Message;
        }

        public static void SendEmail(string password)
        {
            SmtpServerIndex = From.IndexOf("@");
            SmtpServer = "smtp." + From[++SmtpServerIndex..];

            SmtpClient smtp = new SmtpClient();

            smtp.Connect(SmtpServer, 587, SecureSocketOptions.StartTls);

            smtp.Authenticate(From, password);

            smtp.Send(Message);

            smtp.Disconnect(true);
        }
    }
}

using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MassMailWeb.Helpers
{
    public static class EmailHelper
    {
        private static string From { get; set; }
        private static string[] To { get; set; }
        private static MimeMessage Message { get; set; } = new MimeMessage();
        private static List<string> Attachments { get; set; } = new List<string>();
        private static string SmtpServer { get; set; }
        private static int SmtpServerIndex { get; set; }
        private static string WwwRoot { get; set; }
        private static string CurrentDirectory { get; set; }
        private static string FilePath { get; set; }

        public static List<string> SetAttachments(IWebHostEnvironment webHostEnvironment, List<IFormFile> attachments)
        {
            /// <summary>
            /// Method to set attachment(s). It checks if there's attachment, then save each file in wwwroot/Uploads folder and add the path to a List<string>
            /// </summary>

            WwwRoot = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
            CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), WwwRoot);

            if (attachments.Count > 0)
            {
                foreach (IFormFile attachment in attachments)
                {
                    FilePath = Path.Combine(CurrentDirectory, attachment.FileName);

                    using (FileStream stream = new FileStream(Path.Combine(WwwRoot, attachment.FileName), FileMode.Create))
                    {
                        attachment.CopyTo(stream);
                    }

                    Attachments.Add(Path.Combine(CurrentDirectory, attachment.FileName));
                }

                return Attachments;
            }

            return null;
        }

        public static MimeMessage SetMessage(string from, string toField, string subject, string body, bool bccOrNot, bool HtmlOrNot)
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

            if (Attachments != null)
            {
                foreach (string attachment in Attachments)
                {
                    bodyBuilder.Attachments.Add(attachment);
                }
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

            DeleteAttachments();
        }

        private static void DeleteAttachments()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(WwwRoot);

            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }

        }
    }
}

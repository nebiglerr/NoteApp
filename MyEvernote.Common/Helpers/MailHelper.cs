using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MyEvernote.Common.Helpers
{
    public class MailHelper
    {
          public static bool SendMail(string body, string to, string subject, bool isHtml = true)
        {
            return SendMail(body, new List<string> { to }, subject, isHtml);
        }

        public static bool SendMail(string body, List<string> to, string subject, bool isHtml = true)
        {
            bool result = false;

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(ConfigHelpers.Get<string>("MailUser"));

                to.ForEach(x =>
                {
                    message.To.Add(new MailAddress(x));
                });

                message.Subject = subject;  
                message.Body = body;
                message.IsBodyHtml = isHtml;

                using (var smtp = new SmtpClient(
                    ConfigHelpers.Get<string>("MailHost"), 
                    ConfigHelpers.Get<int>("MailPort")))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = 
                        new NetworkCredential(
                            ConfigHelpers.Get<string>("MailUser"), 
                            ConfigHelpers.Get<string>("MailPass"));

                    smtp.Send(message);
                    result = true;
                }
            }
            catch (Exception)
            {

            }

            return result;
        }
    }
}

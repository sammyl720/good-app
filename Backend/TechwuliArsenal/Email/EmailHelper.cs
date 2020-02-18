using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace TechwuliArsenal.Email
{
    public class EmailHelper
    {
        public static bool SendMail(SmtpSection emailSetting, string to, string subject, string body, bool isHtml = true)
        {
            if (emailSetting == null)
            {
                return false;
            }
            
            var emailSender = new EmailSender(new EmailSender.EmailOptions
            {
                Server = emailSetting.Network.Host,
                Port = emailSetting.Network.Port,
                SslEnabled = emailSetting.Network.EnableSsl,
                FromEmail = emailSetting.From,
                UserName = emailSetting.Network.UserName,
                Password = emailSetting.Network.Password
            });
            return emailSender.Send(to, subject, body, isHtml);
        }

        public static string ReplaceWordsHolder(string detail, params KeyValuePair<string, string>[] keyValuePairs)
        {
            return keyValuePairs.Aggregate(detail,
                (current, arg) => current.Replace("{" + arg.Key.ToUpper() + "}", arg.Value));
        }

        private class EmailSender
        {
            public class EmailOptions
            {
                public string Server { get; set; }

                public int Port { get; set; }

                public bool SslEnabled { get; set; }

                public string FromEmail { get; set; }

                public string UserName { get; set; }

                public string Password { get; set; }
            }

            private EmailOptions Options { get; set; }

            public EmailSender(EmailOptions options)
            {
                Options = options;
            }

            public bool Send(string from, string tos, string ccs, string bccs, string subject, string message,
                bool isHtml = true)
            {
                var mailMessage = new MailMessage();
                MailAddress mailAddress;
                if (!string.IsNullOrWhiteSpace(from))
                {
                    mailAddress = new MailAddress(from);
                }
                else
                {
                    mailAddress = new MailAddress(Options.FromEmail);
                }
                if (!string.IsNullOrWhiteSpace(tos))
                {
                    var toList = tos.Split(new [] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
                    if (toList.Length > 0)
                    {
                        foreach (var to in toList)
                        {
                            mailMessage.To.Add(to);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(ccs))
                {
                    var ccList = ccs.Split(new [] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
                    if (ccList.Length > 0)
                    {
                        foreach (var cc in ccList)
                        {
                            mailMessage.CC.Add(cc);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(bccs))
                {
                    var bccList = bccs.Split(new [] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
                    if (bccList.Length > 0)
                    {
                        foreach (var bcc in bccList)
                        {
                            mailMessage.Bcc.Add(bcc);
                        }
                    }
                }
                mailMessage.From = mailAddress;
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = isHtml;

                var smtp = new SmtpClient(Options.Server)
                {
                    Credentials = new NetworkCredential(Options.UserName, Options.Password),
                    Port = Options.Port,
                    EnableSsl = Options.SslEnabled
                };
                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }

            public bool Send(string to, string subject, string message, bool isHtml = true)
            {
                return Send(Options.FromEmail, to, string.Empty, string.Empty, subject, message, isHtml);
            }
        }
    }
}
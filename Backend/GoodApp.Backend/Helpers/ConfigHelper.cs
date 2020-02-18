using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GoodApp.Backend.Helpers
{
    public class ConfigHelper
    {
        public static SmtpSection EmailSetting
        {
            get
            {
                var smtpConfig = WebConfigurationManager.GetSection("system.net/mailSettings/smtp");
                if (smtpConfig != null)
                {
                    var smtp = smtpConfig as SmtpSection;
                    if (smtp != null)
                    {
                        return smtp;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Azure Storage Connection String
        /// </summary>
        public static String AzureStorageConnectionString
        {
            get { return WebConfigurationManager.AppSettings["AzureStorageConnectionString"]; }
        }
    }
}

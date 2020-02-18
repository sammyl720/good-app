using System.Collections.Generic;
using GoodApp.Data.Models;
using TechwuliArsenal.Email;

namespace GoodApp.Backend.Extensions
{
    public static class EmailTemplateExtension
    {
        public static void ReplaceWordsHolder(this EmailTemplate emailTemplate,
            params KeyValuePair<string, string>[] keyValuePairs)
        {
            emailTemplate.Title = EmailHelper.ReplaceWordsHolder(emailTemplate.Title, keyValuePairs);
            emailTemplate.Content = EmailHelper.ReplaceWordsHolder(emailTemplate.Content, keyValuePairs);
        }
    }
}

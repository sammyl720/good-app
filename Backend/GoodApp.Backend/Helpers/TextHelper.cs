using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data;

namespace GoodApp.Backend.Helpers
{
    public static class TextHelper
    {
        public static string ShortString(string text, int len)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.Length > len)
            {
                return text.Substring(0, len) + "...";
            }
            return text;
        }
    }
}

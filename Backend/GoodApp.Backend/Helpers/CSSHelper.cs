using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data;

namespace GoodApp.Backend.Helpers
{
    public static class CSSHelper
    {
        public static string GetUserStatusCss(string status)
        {
            var css = "info";
            Enums.UserStatus enumStatus;
            if (Enum.TryParse(status, out enumStatus))
            {
                switch (enumStatus)
                {
                    case Enums.UserStatus.Active:
                        css = "success";
                        break;
                    case Enums.UserStatus.Inactive:
                        css = "warning";
                        break;
                    case Enums.UserStatus.Invited:
                        css = "info";
                        break;
                    case Enums.UserStatus.Banned:
                        css = "danger";
                        break;
                }
            }
            return css;
        }

        public static string GetChallengeStatusCss(string status)
        {
            var css = "info";
            Enums.ChallengeStatus enumStatus;
            if (Enum.TryParse(status, out enumStatus))
            {
                switch (enumStatus)
                {
                    case Enums.ChallengeStatus.Publish:
                        css = "success";
                        break;
                    case Enums.ChallengeStatus.Draft:
                        css = "warning";
                        break;
                }
            }
            return css;
        }
    }
}

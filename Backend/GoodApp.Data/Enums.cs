using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodApp.Data
{
    public class Enums
    {
        public enum UserStatus
        {
            Active,
            Inactive,
            Banned,
            Invited
        }

        public enum ChallengeType
        {
            Group,
            Individual
        }

        public enum ChallengeStatus
        {
            Draft,
            Publish
        }

        // Used for Chanllenge
        public enum FrequencyType
        {
            ByHour,
            ByDay,
            ByWeek,
            ByMonth,
            ByYear
        }

        public enum ApplicationStatus
        {
            Active,
            Inactive
        }

        public enum ApplicationType
        {
            Mobile,
            Website,
            Admin
        }

        public enum EmailType
        {
            Invitation,
            ResetPassword,
            ConfirmYourAccount
        }

        public enum  RoleType
        {
            Administrator,
            User
        }
    }
}

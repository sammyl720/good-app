using System;
using System.Linq;
using GoodApp.Data.Models;

namespace GoodApp.Backend.Models
{
    public class UserDetailViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name
        {
            get { return string.Join(" ", new[] {FirstName, LastName}); }
        }

        public string PhotoPath { get; set; }

        public string UserId { get; set; }

        public Boolean JoinedGroup { get; set; }

        public DateTime DateCreated { get; set; }

        public static UserDetailViewModel Create(ApplicationUser user)
        {
            var result = new UserDetailViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoPath = user.PhotoPath,
                JoinedGroup = user.JoinedGroups.Any(),
                UserId = user.Id,
                DateCreated = user.CreateDate
            };

            return result;
        }
    }
}
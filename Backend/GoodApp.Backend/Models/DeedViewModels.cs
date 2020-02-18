using System;
using GoodApp.Data.Models;

namespace GoodApp.Backend.Models
{
    public class DeedOverViewModel
    {
        public int TotalDeedCount { get; set; }

        public int NetworkDeedCount { get; set; }

        public int MyTotalDeedCount { get; set; }

        public int FriendsCount { get; set; }
    }

    public class DeedViewModel
    {
        public Guid DeedId { get; set; }

        public Guid ChallengeId { get; set; }

        public string Challenge { get; set; }

        public string ChallengeDescription { get; set; }

        public string Location { get; set; }

        public DateTime DeedTime { get; set; }

        public static DeedViewModel Create(Deed deed)
        {
            var model = new DeedViewModel
            {
                ChallengeId = deed.ChallengeId,
                DeedId = deed.DeedId,
                DeedTime = deed.DeedDate,
                Challenge = deed.Challenge.Name,
                Location = deed.Location
            };

            if (deed.Challenge != null)
            {
                model.Challenge = deed.Challenge.Name;
                model.ChallengeDescription = deed.Challenge.Description;
            }

            return model;
        }
    }
}

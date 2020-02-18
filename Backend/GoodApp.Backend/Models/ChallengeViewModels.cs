using System;
using System.Linq;
using GoodApp.Data.Models;

namespace GoodApp.Backend.Models
{
    public class ChallengeViewModel
    {
        public Guid ChallengeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }

        public DateTime DueDate { get; set; }

        public int CompletedCount { get; set; }

        public int IncompletedCount { get; set; }

        public int Rating { get; set; }

        public string Picture {get; set;}

        public static ChallengeViewModel Create(Challenge challenge, string userId = "")
        {
            var model = new ChallengeViewModel
            {
                ChallengeId = challenge.ChallengeId,
                Count = challenge.Count,
                Description = challenge.Description,
                DueDate = challenge.DueDate,
                Name = challenge.Name,
                Picture = challenge.Picture
            };

            if (!string.IsNullOrEmpty(userId) && challenge.Deeds != null)
            {
                model.CompletedCount = challenge.Deeds.Count(p => p.CreatorId == userId);
                model.IncompletedCount = challenge.Count - model.CompletedCount;
                if (model.IncompletedCount < 0)
                {
                    model.IncompletedCount = 0;
                }

                model.Rating =
                    Convert.ToInt32(
                        Math.Floor(
                            challenge.Deeds.Where(p => p.Rating.HasValue).Average(p => p.Rating).GetValueOrDefault(3)));
            }

            return model;
        }
    }
}
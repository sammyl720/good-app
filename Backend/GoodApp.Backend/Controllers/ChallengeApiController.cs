using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GoodApp.Backend.Api.Controllers;
using GoodApp.Backend.Models;
using GoodApp.Data.Models;
using TechwuliArsenal.GoogleMapsWebServices;
using TechwuliArsenal.GoogleMapsWebServices.Exceptions;
using GoodApp.Data.Repositories;

namespace GoodApp.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/challenges")]
    public class ChallengeApiController : BaseApiController
    {
        [Route("")]
        public IHttpActionResult GetChallenges()
        {
            var challenges = RepositoryProvider.Get<ChallengeRepository>().GetAvailableChallenges(CurrentAccess.UserId);
            var result = challenges.Select(p => ChallengeViewModel.Create(p, CurrentAccess.UserId)).ToList();
            return Ok(result);
        }

        [Route("{challengeId}")]
        public async Task<IHttpActionResult> GetChallenge(Guid challengeId)
        {
            var challenge =
                await
                    RepositoryProvider.Get<ChallengeRepository>().Get(p => p.ChallengeId == challengeId)
                        .Include("Deeds")
                        .FirstOrDefaultAsync();
            if (challenge == null)
            {
                return NotFound();
            }

            var result = ChallengeViewModel.Create(challenge, CurrentAccess.UserId);
            return Ok(result);
        }

        [Route("{challengeId}/comments")]
        public IHttpActionResult GetComments(Guid challengeId, int pageIndex = 0, int pageSize = 0)
        {
            var comments = RepositoryProvider.Get<CommentRepository>().GetComments(challengeId.ToString());
            if (pageSize > 0 && pageIndex >= 0)
            {
                comments = comments.Skip(pageSize * pageIndex).Take(pageSize);
            }

            var result = comments.Select(CommentViewModel.Create);
            return Ok(result);
        }

        [HttpPost]
        [Route("{challengeId}/comments")]
        public IHttpActionResult PostComment(Guid challengeId, CommentBindingModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(challengeId);
                    if (challenge == null)
                    {
                        return NotFound();
                    }

                    var comment = new Comment();
                    comment.Caption = model.Caption;
                    comment.CommentId = Guid.NewGuid();
                    comment.CreateDate = DateTime.Now;
                    comment.ReferenceId = challengeId.ToString();
                    comment.Type = Comment.CommentType.ChallengeComment;
                    comment.UserId = CurrentAccess.UserId;

                    RepositoryProvider.Get<CommentRepository>().Insert(comment);
                    RepositoryProvider.Save();

                    return Ok();
                }
                catch (Exception exception)
                {
                    return InternalServerError(exception);
                }
            }

            return BadRequest(ModelState);
        }

        private async Task AddDeed(Guid challengeId, DateTime deedTime, string location, double lat, double lon,
            string comment, int? rating,
            string recipient)
        {
            var deed = new Deed
            {
                CreatorId = CurrentAccess.UserId,
                CreateDate = DateTime.UtcNow,
                ChallengeId = challengeId,
                DeedId = Guid.NewGuid(),
                DeedDate = deedTime,
                Location = location,
                Lat = lat,
                Lon = lon,
                Comment = comment,
                Rating = rating
            };


            if (!string.IsNullOrEmpty(recipient))
            {
                var recipientUser = await RepositoryProvider.UserStore.FindByIdAsync(recipient);
                if (recipientUser == null)
                {
                    return;
                }

                deed.TaggedUsers.Add(recipientUser);
            }
            RepositoryProvider.Get<DeedRepository>().Insert(deed);            
        }

        [Route("{challengeId}/deeds")]
        [HttpPost]
        public async Task<IHttpActionResult> NewDeed(Guid challengeId, DeedCreateBindingModel model)
        {
            double lat, lon;
            string address;

            if (!model.Lat.HasValue || !model.Lon.HasValue)
            {
                try
                {
                    var location = await Geocoding.RequestAsync(model.Location);
                    lat = location.Lat;
                    lon = location.Lon;
                    address = location.Address;
                }
                catch (GeocodingException exception)
                {
                    return InternalServerError(exception);
                }
            }
            else
            {
                lat = model.Lat.Value;
                lon = model.Lon.Value;
                address = model.Location;
            }

            if (model.TaggedUserIds == null || model.TaggedUserIds.Length == 0)
            {
                await AddDeed(challengeId, model.DeedTime, address, lat, lon, model.Comment, model.Rating, null);
            }
            else
            {
                foreach (var recipient in model.TaggedUserIds.Distinct())
                {
                    await
                        AddDeed(challengeId, model.DeedTime, address, lat, lon, model.Comment, model.Rating, recipient);
                }
            }

            if (!string.IsNullOrEmpty(model.Comment))
            {
                var newComment = new Comment();
                newComment.Caption = model.Comment;
                newComment.CommentId = Guid.NewGuid();
                newComment.CreateDate = DateTime.Now;
                newComment.ReferenceId = challengeId.ToString();
                newComment.Type = Comment.CommentType.ChallengeComment;
                newComment.UserId = CurrentAccess.UserId;

                RepositoryProvider.Get<CommentRepository>().Insert(newComment);
            }

            try
            {
                await RepositoryProvider.SaveAsync();
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
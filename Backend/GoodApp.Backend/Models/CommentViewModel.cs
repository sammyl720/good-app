using GoodApp.Data.Models;

namespace GoodApp.Backend.Models
{
    public class CommentViewModel
    {
        public string Caption { get; set; }

        public string UserName { get; set; }

        public string UserPhotoPath { get; set; }

        public static CommentViewModel Create(Comment comment)
        {
            var model = new CommentViewModel
            {
                Caption = comment.Caption
            };

            if (comment.User != null)
            {
                model.UserName = string.Join(" ", new[] { comment.User.FirstName, comment.User.LastName });
                model.UserPhotoPath = comment.User.PhotoPath;
            }

            return model;
        }
    }
}

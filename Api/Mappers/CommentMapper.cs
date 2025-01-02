using Api.DTOs.Comment;
using Api.Models;

namespace Api.Mappers
{
    public static class CommentMapper
    {

        public static CommentDTO ToCommentDTO(this Comment commentModel)
        {
            return new CommentDTO()
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                ModifiedOn = commentModel.ModifiedOn             
            };
        }
    }
}

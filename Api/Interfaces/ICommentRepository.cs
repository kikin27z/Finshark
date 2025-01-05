using Api.DTOs.Comment;
using Api.Models;

namespace Api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment?> UpdateAsync(int id, Comment commentModel);
        Task<Comment?> DeleteAsync(int id);
    }
}

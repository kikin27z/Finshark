using api.Helpers;
using Api.Data;
using Api.DTOs.Comment;
using Api.Helper;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDBContext _context;

        public CommentRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null)
                return null;

            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();
            return existingComment;
        }


        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = _context.Comments.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol);
            };
            if (queryObject.IsDecsending == true)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.
                Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;
            existingComment.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}

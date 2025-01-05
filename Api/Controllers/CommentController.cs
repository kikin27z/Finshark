using Api.DTOs.Comment;
using Api.Interfaces;
using Api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentsDTO = comments.Select(s => s.ToCommentDTO());
            return Ok(commentsDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null) return NotFound();
            return Ok(comment.ToCommentDTO());
        }

        [HttpPost]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDTO commentDto)
        {
            if (!await _stockRepo.StockExits(stockId))
            {
                BadRequest("Stock doesnt exist");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId);

            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO commentDTO)
        {
            var updateComment = await _commentRepo.UpdateAsync(id, commentDTO.ToCommentFromUpdate());

            if (updateComment == null)
            {
                return NotFound();
            }

            return Ok(updateComment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var commentModel = await _commentRepo.DeleteAsync(id);

            if (commentModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

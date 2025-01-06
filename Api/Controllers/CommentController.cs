using api.Helpers;
using Api.DTOs.Comment;
using Api.Extensions;
using Api.Interfaces;
using Api.Mappers;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;

        public CommentController(ICommentRepository commentRepo,
            IStockRepository stockRepo,
            UserManager<AppUser> userManager,
            IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]CommentQueryObject queryObject)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comments = await _commentRepo.GetAllAsync(queryObject);
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
        [Route("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentDTO commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if(stock == null)
                {
                    return BadRequest("Stock not exists");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            var user = await _userManager.FindByNameAsync(User.GetUserName());

            var commentModel = commentDto.ToCommentFromCreate(stock.Id);
            commentModel.AppUserId = user.Id;
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO commentDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
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

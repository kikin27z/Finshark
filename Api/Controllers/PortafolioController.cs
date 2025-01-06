using Api.Extensions;
using Api.Interfaces;
using Api.Models;
using Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortafolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortafolioRepository _portafolioRepo;
        private readonly IFMPService _fmpService;
        public PortafolioController(UserManager<AppUser> userManager,
            IStockRepository stockRepo,
            IPortafolioRepository portafolioRepo,
            IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portafolioRepo = portafolioRepo;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortafolio()
        {
            //The object User is attach to the ControllerBase class 
            // and its for retrieve the user information from the token claims
           var user = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(user);
            var userPortafolio = await _portafolioRepo.GetUserPortafolio(appUser);
            return Ok(userPortafolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortafolio(string symbol)
        {
            var user = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(user);

            var stock = await _stockRepo.GetBySymbolAsync(symbol);


            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock not exists");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            if (stock == null) return BadRequest("Stock not found");

            var userPortafolio = await _portafolioRepo.GetUserPortafolio(appUser);

            if (userPortafolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Cannot add same stock to the portafolio");



            var portafolio = new Portafolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            var createdPortafolio = await _portafolioRepo.CreateAsync(portafolio);

            if (createdPortafolio == null) return StatusCode(500, "Could not create");
            return CreatedAtAction("GetUserPortafolio", createdPortafolio);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var user = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(user);

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null) return NotFound("Stock not found");


            var userPortafolio = await _portafolioRepo.GetUserPortafolio(appUser);

            if (!userPortafolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Stock not found in the portafolio");

            var deletePortafolio = await _portafolioRepo.DeleteAsync(appUser, symbol);

            if(deletePortafolio == null) return BadRequest( "Could not delete");

            return Ok(deletePortafolio);
        }
    }
}

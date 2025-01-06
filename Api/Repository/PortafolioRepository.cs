using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class PortafolioRepository : IPortafolioRepository
    {
        private readonly AppDBContext _context;
        public PortafolioRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Portafolio> CreateAsync(Portafolio portafolio)
        {
            await _context.Portafolios.AddAsync(portafolio);
            await _context.SaveChangesAsync();
            return portafolio;
        }

        public async Task<Portafolio?> DeleteAsync(AppUser user, string symbol)
        {
            var portafolio = _context.Portafolios
                .FirstOrDefault(p => p.AppUserId == user.Id && p.Stock.Symbol == symbol);

            if (portafolio == null) return null;

            _context.Portafolios.Remove(portafolio);
            await _context.SaveChangesAsync();
            return portafolio;
        }

        public async Task<List<Stock>> GetUserPortafolio(AppUser user)
        {
            return await _context.Portafolios
                .Where(p => p.AppUserId == user.Id)
                .Select(p => new Stock
                {
                    Id = p.StockId,
                    Symbol = p.Stock.Symbol,
                    CompanyName = p.Stock.CompanyName,
                    Purchase = p.Stock.Purchase,
                    LastDiv = p.Stock.LastDiv,
                    Industry = p.Stock.Industry,
                    MarketCap = p.Stock.MarketCap

                }).ToListAsync();
        }
    }
}

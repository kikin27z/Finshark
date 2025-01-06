using Api.Data;
using Api.DTOs.Stock;
using Api.Helper;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class StockRepository : IStockRepository
    {

        private readonly AppDBContext _context;

        public StockRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAll(QueryObject query)
        {
            var stocks = _context.Stocks
                .Include(s => s.Comments)
                .ThenInclude(a => a.AppUser)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            
            }

            if (!string.IsNullOrWhiteSpace(query.SortingBy))
            {
                if(query.SortingBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.Descending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);   
                }

                if(query.SortingBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.Descending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);   

                }
            }

            var skipNumber = (query.NumPage - 1) * query.NumSize;
            stocks = stocks.Skip(skipNumber).Take(query.NumSize);

            return await stocks.ToListAsync();
                
        }

        public async Task<Stock?> GetById(int id)
        {
            return await _context.Stocks
                .Include(s => s.Comments)
                .ThenInclude(a => a.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol);
        }

        public async Task<bool> StockExits(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
            {
                return null;
            }

            stockModel.Symbol = stockDTO.Symbol;
            stockModel.CompanyName = stockDTO.CompanyName;
            stockModel.Purchase = stockDTO.Purchase;
            stockModel.LastDiv = stockDTO.LastDiv;
            stockModel.Industry = stockDTO.Industry;
            stockModel.MarketCap = stockDTO.MarketCap;

            await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}

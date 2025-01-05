using Api.DTOs.Stock;
using Api.Helper;
using Api.Models;

namespace Api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAll(QueryObject query);

        Task<Stock?> GetById(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExits(int id);
    }
}

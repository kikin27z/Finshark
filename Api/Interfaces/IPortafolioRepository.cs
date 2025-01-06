using Api.Models;

namespace Api.Interfaces
{
    public interface IPortafolioRepository
    {
        Task<List<Stock>> GetUserPortafolio(AppUser user);

        Task<Portafolio> CreateAsync(Portafolio portafolio);
        Task<Portafolio?> DeleteAsync(AppUser user, string symbol);
    }
}

using System.ComponentModel.DataAnnotations;

namespace Api.DTOs.Stock
{
    public class UpdateStockRequestDTO
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters long")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "Company Name cannot be over 10 characters long")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 10000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 10000000)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters long")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 10000000)]
        public long MarketCap { get; set; }
    }
}

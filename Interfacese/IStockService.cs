using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Models;

namespace InventoryManagementWithExpirationDatesSystem.Interfacese
{
    public interface IStockService
    {
        Task<StockDTO> GetStockByIdAsync(int stockId);
        Task<IEnumerable<StockDTO>> GetAllStocksAsync();
        Task<StockDTO> AddStockAsync(StockDTO stockDTO);
        Task<StockDTO> UpdateStockAsync(int stockId, StockDTO stockDTO);
        Task<bool> DeleteStockAsync(int stockId);
    }

}

using InventoryManagementWithExpirationDatesSystem.Models;

namespace InventoryManagementWithExpirationDatesSystem.Interfacese
{
    public interface IStockService
    {
        Task<Stock> GetStockByIdAsync(int stockId);
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> AddStockAsync(Stock stock);
        Task<Stock> UpdateStockAsync(Stock stock);
        Task<bool> DeleteStockAsync(int stockId);
    }

}

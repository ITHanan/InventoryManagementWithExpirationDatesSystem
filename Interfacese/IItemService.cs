using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Models;
using InventoryManagementWithExpirationDatesSystem.DTOs;  // Import DTO namespace


namespace InventoryManagementWithExpirationDatesSystem.Interfaces  // Fixed namespace
{
    public interface IItemService
    {
        Task<ItemDTO> GetItemByIdAsync(int itemId);      // Get a single item by its ID
        Task<IEnumerable<ItemDTO>> GetAllItemsAsync();   // Get a list of all items
        Task<ItemDTO> AddItemAsync(ItemDTO itemDto);     // Add a new item
        Task<ItemDTO> UpdateItemAsync(int id, ItemDTO itemDto);  // Update an existing item
        Task<bool> DeleteItemAsync(int itemId);          // Delete an item by its ID
        Task<ItemDTO> UpdateItemUnitPriceAsync(int id, decimal newUnitPrice);




    }
}
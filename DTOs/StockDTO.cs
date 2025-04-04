using InventoryManagementWithExpirationDatesSystem.Models;
using System;

namespace InventoryManagementWithExpirationDatesSystem.DTOs
{
    public class StockDTO
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ReceivedDate { get; set; } // Nullable for optional received date //////////////////////////////////

        // Navigation properties (optional, include if needed)
        //public ItemDTO? Item { get; set; }
        //public SupplierDTO? Supplier { get; set; }
    }
}

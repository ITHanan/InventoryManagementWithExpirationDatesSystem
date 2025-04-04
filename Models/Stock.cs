using System;
using System.Collections.Generic;

namespace InventoryManagementWithExpirationDatesSystem.Models;

public partial class Stock
{
    public int StockId { get; set; }

    public int ItemId { get; set; }

    public int SupplierId { get; set; }

    public int Quantity { get; set; }

    public DateTime ExpiryDate { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}

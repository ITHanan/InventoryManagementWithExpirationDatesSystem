using System;
using System.Collections.Generic;

namespace InventoryManagementWithExpirationDatesSystem.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}

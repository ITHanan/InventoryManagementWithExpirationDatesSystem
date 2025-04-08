using Bogus;
using InventoryManagementWithExpirationDatesSystem.Models;

namespace InventoryManagementWithExpirationDatesSystem.Database
{
    public static class DataSeeder
    {
        public static void SeedData(WarehouseManagementSystemContext context, int itemCount, int stockCount = 100, int supplierCount = 10)
        {
           if (context.Items.Any()) return;

            var faker = new Faker<Item>()
                .RuleFor(i => i.ItemName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Category, f => f.Commerce.Department())
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 100))
                .RuleFor(i => i.Stocks, f => new List<Stock>());

            var items = faker.Generate(itemCount);
            context.Items.AddRange(items);
            context.SaveChanges();

            // --- Seed suppliers BEFORE stocks ---
            var supplierFaker = new Faker<Supplier>()
                .RuleFor(s => s.SupplierName, f => f.Company.CompanyName())
                .RuleFor(s => s.ContactPerson, f => f.Name.FullName())
                .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber());

            var suppliers = supplierFaker.Generate(supplierCount);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            // Get valid ItemIds and SupplierIds from DB
            var itemIds = context.Items.Select(i => i.ItemId).ToList();
            var supplierIds = context.Suppliers.Select(s => s.SupplierId).ToList();

            var stockFaker = new Faker<Stock>()
                .RuleFor(s => s.ItemId, f => f.PickRandom(itemIds))
                .RuleFor(s => s.SupplierId, f => f.PickRandom(supplierIds)) // set valid SupplierId
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(s => s.ExpiryDate, f => f.Date.Future())
                .RuleFor(s => s.ReceivedDate, f => f.Date.Recent());

            var stocks = stockFaker.Generate(stockCount);
            context.Stocks.AddRange(stocks);
            context.SaveChanges();

            Console.WriteLine($" Seeded {itemCount} items, {supplierCount} suppliers, and {stockCount} stocks.");
        }

    }
}

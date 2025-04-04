using Bogus;
using InventoryManagementWithExpirationDatesSystem.Models;

namespace InventoryManagementWithExpirationDatesSystem.Database
{
    public static class DataSeeder
    {
        public static void SeedData(WarehouseManagementSystemContext context, int itemCount, int stockCount = 100)
        {
            if (context.Items.Any()) return;

            var faker = new Faker<Item>()
                .RuleFor(i => i.ItemName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Category, f => f.Commerce.Department())
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 100))
                .RuleFor(i => i.Stocks, f => new List<Stock>());

            var items = faker.Generate(itemCount);

            // Save items first so that valid ItemIds exist
            context.Items.AddRange(items);
            context.SaveChanges();

            // Get valid ItemIds from database
            var itemIds = context.Items.Select(i => i.ItemId).ToList();

            var stockFaker = new Faker<Stock>()
                .RuleFor(s => s.ItemId, f => f.PickRandom(itemIds)) // Ensure valid ItemId
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(s => s.ExpiryDate, f => f.Date.Future());

            var stocks = stockFaker.Generate(stockCount);

            // Add stocks to database
            context.Stocks.AddRange(stocks);
            context.SaveChanges();

            Console.WriteLine($"Added {itemCount} new items and {stockCount} new stocks into the database.");
        }
    }
}

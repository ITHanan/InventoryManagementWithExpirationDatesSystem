using Bogus;
using InventoryManagementWithExpirationDatesSystem.Models;

namespace InventoryManagementWithExpirationDatesSystem.Database
{
    public static class DataSeeder
    {
        public static void SeedData(WarehouseManagementSystemContext context, int itemCount = 50)
        {
           

            var faker = new Faker<Item>()
                .RuleFor(i => i.ItemName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Category, f => f.Commerce.Department())
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 100))
                .RuleFor(i => i.Stocks, f => new List<Stock>());

            var items = faker.Generate(itemCount);

            // Add to database
            context.Items.AddRange(items);
            context.SaveChanges();

            Console.WriteLine($"Added  {itemCount} new items into the database.");
        }
    }
}
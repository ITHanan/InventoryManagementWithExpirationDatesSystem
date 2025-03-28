using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using InventoryManagementWithExpirationDatesSystem;
using AutoMapper;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.Validations;
using InventoryManagementWithExpirationDatesSystem.Interfaces;
using InventoryManagementWithExpirationDatesSystem.Services;




namespace InventoryManagementWithExpirationDatesSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<IItemService, ItemService>();



            // Add FluentValidation and register validators
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<ItemDTOValidator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<WarehouseManagementSystemContext>(options => options.UseSqlServer("Server=HANAN\\SQLEXPRESS;Database=WarehouseManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;"));

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WarehouseManagementSystemContext>();

                // Ensure database exists
                context.Database.EnsureCreated();

                // Seed only if no data exists
                DataSeeder.SeedData(context, 50);
                //context.Database.Migrate();

 
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

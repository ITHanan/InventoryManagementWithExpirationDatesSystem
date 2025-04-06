using FluentValidation;
using FluentValidation.AspNetCore;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfaces;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Servases;
using InventoryManagementWithExpirationDatesSystem.Services;
using InventoryManagementWithExpirationDatesSystem.Validations;
using InventoryManagementWithExpirationDatesSystem.JWTHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InventoryManagementWithExpirationDatesSystem.Extensions;

namespace InventoryManagementWithExpirationDatesSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();

            // Dependency Injections
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();

            // DTO Validators
            builder.Services.AddScoped<IValidator<StockDTO>, StockDTOValidator>();
            builder.Services.AddScoped<IValidator<ItemDTO>, ItemDTOValidator>();
            builder.Services.AddScoped<IValidator<SupplierDTO>, SupplierDTOValidator>();

            // FluentValidation Setup
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<StockDTOValidator>();

            builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>();


            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddSwaggerWithJwt();


            // DbContext
            builder.Services.AddDbContext<WarehouseManagementSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HANAN_IDENTITY")));

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WarehouseManagementSystemContext>();
                context.Database.EnsureCreated();

                if (!context.Items.Any() || !context.Stocks.Any() || !context.Suppliers.Any())
                {
                    DataSeeder.SeedData(context, 50);
                }
            }

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); // <- make sure this comes before Authorization
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}


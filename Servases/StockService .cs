using AutoMapper;
using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementWithExpirationDatesSystem.Servases
{
    public class StockService : IStockService
    {
        private readonly WarehouseManagementSystemContext _context; // assuming you're using Entity Framework
        private readonly IMapper _mapper;
        private readonly IValidator<StockDTO> _validator;

        public StockService(WarehouseManagementSystemContext context, IMapper mapper, IValidator<StockDTO> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<StockDTO> AddStockAsync(StockDTO stockDTO)
        {
            try
            {
                // Check if Item exists 
                var itemExists = await _context.Items.AnyAsync(i => i.ItemId == stockDTO.ItemId);
                if (!itemExists)
                {
                    throw new ArgumentException(" Cannot add stock: The referenced ItemId does not exist.");
                }

                // Check if Supplier exists
                var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == stockDTO.SupplierId);
                if (!supplierExists)
                {
                    throw new ArgumentException(" Cannot add stock: The referenced SupplierId does not exist.");
                }

                var stock = _mapper.Map<Stock>(stockDTO);
                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();

                Console.WriteLine(" Stock added successfully.");
                return _mapper.Map<StockDTO>(stock);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error while adding stock: {ex.Message}");
                throw new ApplicationException("An error occurred while adding the stock", ex);
            }
        }



        public async Task<bool> DeleteStockAsync(int stockId)
        {
            var stock = await _context.Stocks.FindAsync(stockId);
            if (stock == null) return false;
            
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }



        //public async Task<IEnumerable<StockDTO>> GetAllStocksAsync()
        //{
        //    var stocks = await _context.Stocks.Include(s => s.Supplier).ToListAsync();
        //    return _mapper.Map<IEnumerable<StockDTO>>(stocks);
        //}

        public async Task<IEnumerable<StockDTO>> GetAllStocksAsync()
        {
            var stocks = await _context.Stocks.FromSqlRaw("SELECT * FROM Stock").ToListAsync();
            return _mapper.Map<IEnumerable<StockDTO>>(stocks);
        }

        //public async Task<IEnumerable<StockDTO>> GetAllStocksAsync()
        //{
        //    var stocks = await _context.Stocks
        //        .Include(s => s.Item)    // Include related Item data
        //        .Include(s => s.Supplier) // Include related Supplier data
        //        .ToListAsync();

        //    return _mapper.Map<IEnumerable<StockDTO>>(stocks);
        //}




        public async Task<StockDTO> GetStockByIdAsync(int stockId)
        {



            var stock = await _context.Stocks.Include(s => s.Item).FirstOrDefaultAsync(s => s.StockId == stockId);
            return stock == null ? null : _mapper.Map<StockDTO>(stock);
        }




        public async Task<StockDTO> UpdateStockAsync(int stockId, StockDTO stockDTO)
        {
            try
            {
                var existingStock = await _context.Stocks.FindAsync(stockId);

                if (existingStock == null) return null;


                _mapper.Map(stockDTO, existingStock); // Update item properties from DTO
              //  _context.Stocks.Update(existingStock);
                await _context.SaveChangesAsync();
                return _mapper.Map<StockDTO>(existingStock);

            }
            catch (Exception ex)
            {
                // Handle any errors (logging can be added here)
                throw new Exception($"An error occurred while updating stock: {ex.Message}");
            }
        }


    }
}
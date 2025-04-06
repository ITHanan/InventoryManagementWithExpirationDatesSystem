using AutoMapper;
using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfaces;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Models;
using InventoryManagementWithExpirationDatesSystem.Validations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementWithExpirationDatesSystem.Services
{
    public class ItemService : IItemService
    {
        private readonly WarehouseManagementSystemContext _context; // assuming you're using Entity Framework
        private readonly IValidator<ItemDTO> _validator;
        private readonly IMapper _mapper;


        public ItemService(WarehouseManagementSystemContext context, IMapper mapper, IValidator<ItemDTO> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ItemDTO> GetItemByIdAsync(int itemId)
        {
            var item = await _context.Items.Include(i => i.Stocks).FirstOrDefaultAsync(i => i.ItemId == itemId);
            return item == null ? null : _mapper.Map<ItemDTO>(item);
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync()
        {
            var items = await _context.Items.Include(i => i.Stocks).ToListAsync();
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public async Task<ItemDTO> AddItemAsync(ItemDTO itemdto)
        {
            var validationResult = await _validator.ValidateAsync(itemdto);


            var item = _mapper.Map<Item>(itemdto);
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task<ItemDTO> UpdateItemAsync(int id, ItemDTO itemDto)
        {

            var item = await _context.Items.FindAsync(id);
            if (item == null) return null;


            _mapper.Map(itemDto, item); // Update item properties from DTO
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task<bool> DeleteItemAsync(int itemId)
        {

            var item = await _context.Items.FindAsync(itemId);
            if (item == null) return false;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<ItemDTO> UpdateItemUnitPriceAsync(int id, decimal newUnitPrice)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return null;

            // Validate UnitPrice
            if (newUnitPrice <= 0) throw new FluentValidation.ValidationException("Unit price must be greater than 0.");

            item.UnitPrice = newUnitPrice;
            await _context.SaveChangesAsync();

            return _mapper.Map<ItemDTO>(item);
        }

        public async Task<IEnumerable<ItemDTO>> GetFilteredAndSortedItemsAsync(string? filterOrCategory, string? sortOrder, string? sortBy, int? itemId)
        {
            var items = await _context.Items.ToListAsync();

            var itemDTOs = items.Select(item => new ItemDTO
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Category = item.Category,
                UnitPrice = (decimal)item.UnitPrice
            });

            // Filter by ItemId, ItemName, or Category
            if (itemId.HasValue)
            {
                itemDTOs = itemDTOs.Where(i => i.ItemId == itemId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(filterOrCategory))
            {
                itemDTOs = itemDTOs.Where(i =>
                    i.ItemName.Contains(filterOrCategory, StringComparison.OrdinalIgnoreCase) ||
                    i.Category.Contains(filterOrCategory, StringComparison.OrdinalIgnoreCase));
            }

            // Normalize parameters
            var sortField = sortBy?.ToLower() ?? "itemname";
            var sortDir = sortOrder?.ToLower() ?? "asc";

            // Dynamic sorting
            itemDTOs = (sortField, sortDir) switch
            {
                ("unitprice", "desc") => itemDTOs.OrderByDescending(i => i.UnitPrice),
                ("unitprice", _) => itemDTOs.OrderBy(i => i.UnitPrice),

                ("itemname", "desc") => itemDTOs.OrderByDescending(i => i.ItemName),
                _ => itemDTOs.OrderBy(i => i.ItemName) // Default
            };

            return itemDTOs;
        }


    }
}

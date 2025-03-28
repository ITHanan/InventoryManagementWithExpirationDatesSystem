using AutoMapper;
using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfaces;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Models;
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
    }
}

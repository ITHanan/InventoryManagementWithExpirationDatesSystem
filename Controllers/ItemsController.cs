using AutoMapper;
using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfaces;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementWithExpirationDatesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/Items
        [HttpGet("Get-all-Item")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        // GET: api/Items/5
        [HttpGet("{id}Get-BY-ID")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, ItemDTO itemDTO)
        {
            if (id != itemDTO.ItemId)
            {
                return BadRequest();
            }

            await _itemService.UpdateItemAsync(id, itemDTO);
            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostItem(ItemDTO itemDTO)
        {
            var createdItem = await _itemService.AddItemAsync(itemDTO);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.ItemId }, createdItem);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/unitprice")]
        public async Task<IActionResult> UpdateItemUnitPrice(int id, [FromBody] decimal newUnitPrice)
        {
            var updatedItem = await _itemService.UpdateItemUnitPriceAsync(id, newUnitPrice);
            return updatedItem == null 
                ? NotFound($"Item with ID {id} not found.") 
                : Ok($"Item with ID {id} updated to new UnitPrice: {updatedItem.UnitPrice}");
        }

    }
}



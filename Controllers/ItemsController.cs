using AutoMapper;
using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfaces;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
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


        /// <summary>
        /// Search items with optional filters and sorting options.
        /// </summary>
        /// <param name="filter">Filter by item name or category</param>
        /// <param name="sortOrder">Sorting direction (asc/desc)</param>
        /// <param name="sortBy">Field to sort by (ItemName, UnitPrice)</param>
        /// <param name="itemId">Filter by ItemId</param>
        /// <returns>List of filtered and sorted items</returns>
        [HttpGet("search")]
        public async Task<IActionResult> GetFilteredAndSortedItems(
            [FromQuery] string? filter,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? sortBy = "itemname",
            [FromQuery] int? itemId = null)
        {
            var items = await _itemService.GetFilteredAndSortedItemsAsync(filter, sortOrder, sortBy, itemId);
            return Ok(items);
        }

    }
}



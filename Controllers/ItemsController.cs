using AutoMapper;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementWithExpirationDatesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly WarehouseManagementSystemContext _context;
        private readonly IMapper _mapper;


        public ItemsController(WarehouseManagementSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/Items
        [HttpGet("Get-all-Item")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ItemDTO>>(items));
        }






        // GET: api/Items/5
        [HttpGet("{id}Get-BY-ID")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ItemDTO>(item));
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, ItemDTO itemDTO)
        {
            if (id != itemDTO.ItemId)
            {
                return BadRequest();
            }

            var item = _mapper.Map<Item>(itemDTO);
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostItem(ItemDTO itemDTO)
        {
            var itemThatWeWantToUpdate = _mapper.Map<Item>(itemDTO);
            _context.Items.Add(itemThatWeWantToUpdate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = itemThatWeWantToUpdate.ItemId }, _mapper.Map<ItemDTO>(itemThatWeWantToUpdate));
        }


        // PATCH: api/Items/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateItemName(int id, [FromBody] string itemNameUpdated)
        {
            // Find the item by ID
            var itemThantWeWantToFindByID= await _context.Items.FindAsync(id);

            if (itemThantWeWantToFindByID == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }

            // Update the item name
            itemThantWeWantToFindByID.ItemName = itemNameUpdated;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok($"Item with ID {id} has been updated to '{itemThantWeWantToFindByID.ItemName}'.");
        }



        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var itemThatWeWantToDelet = await _context.Items.FindAsync(id);
            if (itemThatWeWantToDelet == null)
            {
                return NotFound();
            }

            // Find all stock records related to this item
            var relatedStock = _context.Stocks.Where(s => s.ItemId == id).ToList();

            // Iterate through each stock record and remove it
            foreach (var stock in relatedStock)
            {
                _context.Stocks.Remove(stock);
            }

            _context.Items.Remove(itemThatWeWantToDelet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}

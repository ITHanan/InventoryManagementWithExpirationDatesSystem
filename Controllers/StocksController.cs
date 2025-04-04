using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementWithExpirationDatesSystem.Models;
using InventoryManagementWithExpirationDatesSystem.Database;
using AutoMapper;
using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfacese;

namespace InventoryManagementWithExpirationDatesSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IValidator<StockDTO> _validator;


        public StocksController(IStockService stockService, IValidator<StockDTO> validator)
        {
            _stockService = stockService;
            _validator = validator;


        }



        // GET: api/stocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetAllStocks()
        {
            var stocks = await _stockService.GetAllStocksAsync();
            return Ok(stocks);
        }






        // GET: api/Stocks/5
        [HttpGet("{id}Get-Stock-By-ID")]
        public async Task<ActionResult<StockDTO>> GetStock(int id)
        {
            var stock = await _stockService.GetStockByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }



        // PUT: api/Stocks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, StockDTO stockDTO)
        {
            if (id != stockDTO.StockId)
            {
                return BadRequest("Stock ID mismatch.");
            }

            try
            {
                var updatedStock = await _stockService.UpdateStockAsync(id, stockDTO);
                if (updatedStock == null)
                {
                    return NotFound($"Stock with ID {id} not found.");
                }

                // Optionally, return the updated stock details instead of just a NoContent response
                return Ok(updatedStock); // 200 OK with updated stock details
            }
            catch (Exception ex)
            {
                // Log exception and return a generic server error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(StockDTO), StatusCodes.Status201Created)] // Tells Swagger the expected response body
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For BadRequest responses

        public async Task<ActionResult<StockDTO>> PostStock(StockDTO stockDTO)
        {
            if (stockDTO == null)
            {
                return BadRequest("StockDTO cannot be null.");
            }

            // Validate model using FluentValidation
            var validationResult = await _validator.ValidateAsync(stockDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _stockService.AddStockAsync(stockDTO);

            return CreatedAtAction(nameof(GetStock), new { id = result.StockId }, result);//////////////////////////////
        }



        // DELETE: api/Stocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _stockService.DeleteStockAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

          
            return NoContent();
        }

       
    }
}

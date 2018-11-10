using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDJaya.Models;
using PDJaya.Service.Helpers;

namespace PDJaya.Service.Controllers
{
    /// <summary>
    /// Manage Market Data
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class MarketsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public MarketsController(PDJayaDB context)
        {
            _context = context;
        }

        // GET: api/Markets
        [HttpGet]
        public async Task<IActionResult> GetMarkets()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.Markets.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        // GET: api/Markets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarket([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var market = await _context.Markets.FindAsync(id);

            if (market == null)
            {
                return NotFound();
            }

            return Ok(market);
        }

        // PUT: api/Markets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarket([FromRoute] long id, [FromBody] Market market)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != market.Id)
            {
                return BadRequest();
            }

            _context.Entry(market).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarketExists(id))
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

        // POST: api/Markets
        [HttpPost]
        public async Task<IActionResult> PostMarket([FromBody] Market market)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Markets.Add(market);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMarket", new { id = market.Id }, market);
        }

        // DELETE: api/Markets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var market = await _context.Markets.FindAsync(id);
            if (market == null)
            {
                return NotFound();
            }

            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();

            return Ok(market);
        }

        private bool MarketExists(long id)
        {
            return _context.Markets.Any(e => e.Id == id);
        }
    }
}
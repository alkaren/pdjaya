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
    /// Manage Tenant Cards
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class TenantCardsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public TenantCardsController(PDJayaDB context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Daftar Kartu By Store No
        /// </summary>
        /// <param name="StoreNo"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetCardbyStoreNo(string StoreNo)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.TenantCards
                            where x.StoreNo == StoreNo
                            orderby x.Id ascending
                            select x;
                hasil.Data = datas.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteCard([FromBody] TenantCard Card)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                if (Card != null)
                {
                 
                    var datas = from x in _context.TenantCards
                                where x.CardID == Card.CardID
                                select x;

                    if (datas != null)
                    {
                        foreach (var item in datas)
                        {
                            _context.TenantCards.Remove(item);
                        }
                        var res = await _context.SaveChangesAsync();
                        hasil.Data = true;
                        hasil.IsSucceed = res > 0 ? true : false;
                    }
                    else
                    {
                        hasil.IsSucceed = false;
                    }
                }
                else
                {
                    hasil.IsSucceed = false;
                }

            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteCards([FromBody] List<TenantCard> Cards)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                if (Cards != null){
                    HashSet<string> CardNos = new HashSet<string>();
                    Cards.ForEach( x => { CardNos.Add(x.CardNo); } );
                    var datas = from x in _context.TenantCards
                                where CardNos.Contains(x.CardID)
                                select x;
                    if (datas != null)
                    {
                        foreach (var item in datas)
                        {
                            _context.TenantCards.Remove(item);
                        }
                        var res = await _context.SaveChangesAsync();
                        hasil.Data = true;
                        hasil.IsSucceed = res > 0 ? true : false;
                    }
                    else
                    {
                        hasil.IsSucceed = false;
                    }
                }
                else
                {
                    hasil.IsSucceed = false;
                }
                
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }
        // GET: api/TenantCards
        [HttpGet]
        public async Task<IActionResult> GetTenantCards()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = _context.TenantCards;
                hasil.Data = datas.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
           
        }

        // GET: api/TenantCards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantCard([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantCard = await _context.TenantCards.FindAsync(id);

            if (tenantCard == null)
            {
                return NotFound();
            }

            return Ok(tenantCard);
        }

        // PUT: api/TenantCards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenantCard([FromRoute] long id, [FromBody] TenantCard tenantCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenantCard.Id)
            {
                return BadRequest();
            }

            _context.Entry(tenantCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantCardExists(id))
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

        // POST: api/TenantCards
        [HttpPost]
        public async Task<IActionResult> PostTenantCard([FromBody] TenantCard tenantCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TenantCards.Add(tenantCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenantCard", new { id = tenantCard.Id }, tenantCard);
        }

        // DELETE: api/TenantCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenantCard([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantCard = await _context.TenantCards.FindAsync(id);
            if (tenantCard == null)
            {
                return NotFound();
            }

            _context.TenantCards.Remove(tenantCard);
            await _context.SaveChangesAsync();

            return Ok(tenantCard);
        }

        private bool TenantCardExists(long id)
        {
            return _context.TenantCards.Any(e => e.Id == id);
        }
    }
}
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
    /// Data Tagihan
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class BillsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public BillsController(PDJayaDB context)
        {
            _context = context;
        }



        /// <summary>
        /// Dapatkan data tagihan berdasarkan code transaksi dan store number
        /// </summary>
        /// <param name="CurrentDate"></param>
        /// <param name="TransactionCode"></param>
        /// <param name="StoreNo"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetBillByTransactionCode(DateTime CurrentDate, string TransactionCode,string StoreNo)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.Bills
                            where  x.CreatedDate<=CurrentDate && !x.IsPaid && x.TransactionCode == TransactionCode && x.StoreNo == StoreNo
                            orderby x.CreatedDate ascending
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

        // GET: api/Bills
        [HttpGet]
        public async Task<IActionResult> GetBills()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = _context.Bills;
                hasil.Data = datas.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        // GET: api/Bills/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBill([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(bill);
        }

        // PUT: api/Bills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill([FromRoute] long id, [FromBody] Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bill.Id)
            {
                return BadRequest();
            }

            _context.Entry(bill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
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

        // POST: api/Bills
        [HttpPost]
        public async Task<IActionResult> PostBill([FromBody] Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBill", new { id = bill.Id }, bill);
        }

        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return Ok(bill);
        }

        private bool BillExists(long id)
        {
            return _context.Bills.Any(e => e.Id == id);
        }
    }
}
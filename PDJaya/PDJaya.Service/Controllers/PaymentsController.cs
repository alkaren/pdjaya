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
    /// Data Pembayaran
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public PaymentsController(PDJayaDB context)
        {
            _context = context;
        }

        /// <summary>
        /// Sync payment data from device to server
        /// </summary>
        /// <param name="PaymentList"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> PushPaymentData([FromBody]List<Payment> PaymentList)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                HashSet<string> TransactionIDS = new HashSet<string>();
                foreach (var item in PaymentList)
                {
                    _context.Payments.Add(item);
                    TransactionIDS.Add(item.TransactionID); // this is BillID
                }
                //update bill flag
                var updatedBills = from x in _context.Bills
                                   where TransactionIDS.Contains(x.BillID)
                                   select x;
              
                foreach(var item in updatedBills)
                {
                    item.IsPaid = true;
                   
                }
               
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        // GET: api/Payments
        [HttpGet]
        public IEnumerable<Payment> GetPayments()
        {
            return _context.Payments;
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment([FromRoute] long id, [FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> PostPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return Ok(payment);
        }

        private bool PaymentExists(long id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
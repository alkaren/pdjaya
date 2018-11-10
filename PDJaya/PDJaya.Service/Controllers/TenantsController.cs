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
    /// 
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class TenantsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public TenantsController(PDJayaDB context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTenantByStoreNo(string StoreNo)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.TenantCards
                            where x.StoreNo == StoreNo
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
        // GET: api/Tenants
        [HttpGet]
        public async Task<IActionResult> GetTenants()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.Tenants.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        // GET: api/Tenants/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenant([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return NotFound();
            }

            return Ok(tenant);
        }

        // PUT: api/Tenants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant([FromRoute] long id, [FromBody] Tenant tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenant.Id)
            {
                return BadRequest();
            }

            _context.Entry(tenant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(id))
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

        // POST: api/Tenants
        [HttpPost]
        public async Task<IActionResult> PostTenant([FromBody] Tenant tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenant", new { id = tenant.Id }, tenant);
        }

        // DELETE: api/Tenants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();

            return Ok(tenant);
        }

        private bool TenantExists(long id)
        {
            return _context.Tenants.Any(e => e.Id == id);
        }
    }
}
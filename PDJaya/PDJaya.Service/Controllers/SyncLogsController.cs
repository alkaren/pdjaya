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
    /// Manage Sync Log Data from device to server
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncLogsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public SyncLogsController(PDJayaDB context)
        {
            _context = context;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> PushSyncLogs([FromBody]List<SyncLog> SyncLogList)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                foreach (var item in SyncLogList)
                {
                    _context.SyncLogs.Add(item);

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

        // GET: api/SyncLogs
        [HttpGet]
        public IEnumerable<SyncLog> GetSyncLogs()
        {
            return _context.SyncLogs;
        }

        // GET: api/SyncLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSyncLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var syncLog = await _context.SyncLogs.FindAsync(id);

            if (syncLog == null)
            {
                return NotFound();
            }

            return Ok(syncLog);
        }

        // PUT: api/SyncLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSyncLog([FromRoute] long id, [FromBody] SyncLog syncLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != syncLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(syncLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SyncLogExists(id))
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

        // POST: api/SyncLogs
        [HttpPost]
        public async Task<IActionResult> PostSyncLog([FromBody] SyncLog syncLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SyncLogs.Add(syncLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSyncLog", new { id = syncLog.Id }, syncLog);
        }

        // DELETE: api/SyncLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSyncLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var syncLog = await _context.SyncLogs.FindAsync(id);
            if (syncLog == null)
            {
                return NotFound();
            }

            _context.SyncLogs.Remove(syncLog);
            await _context.SaveChangesAsync();

            return Ok(syncLog);
        }

        private bool SyncLogExists(long id)
        {
            return _context.SyncLogs.Any(e => e.Id == id);
        }
    }
}
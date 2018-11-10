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
    /// Manage App Log in server
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class AppLogsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public AppLogsController(PDJayaDB context)
        {
            _context = context;
        }

        /// <summary>
        /// Push device app log to server
        /// </summary>
        /// <param name="AppLogs"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> PushAppLogs([FromBody]List<AppLog> AppLogs)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                foreach (var item in AppLogs)
                {
                    _context.AppLogs.Add(item);

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

        // GET: api/AppLogs
        [HttpGet]
        public async Task<IActionResult> GetAppLogs()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.AppLogs;
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
            
        }

        // GET: api/AppLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppLog([FromRoute] long id)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                if (!ModelState.IsValid)
                {
                    hasil.IsSucceed = false;
                    hasil.ErrorMessage = "state is not valid";
                }
                else
                {

                    var appLog = await _context.AppLogs.FindAsync(id);

                    if (appLog == null)
                    {
                        hasil.IsSucceed = false;
                        hasil.ErrorMessage = "not found";
                    }
                    else
                    {
                        hasil.Data = appLog;
                    }
                }
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
            
        }

        // PUT: api/AppLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppLog([FromRoute] long id, [FromBody] AppLog appLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(appLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppLogExists(id))
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

        // POST: api/AppLogs
        [HttpPost]
        public async Task<IActionResult> PostAppLog([FromBody] AppLog appLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AppLogs.Add(appLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppLog", new { id = appLog.Id }, appLog);
        }

        // DELETE: api/AppLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appLog = await _context.AppLogs.FindAsync(id);
            if (appLog == null)
            {
                return NotFound();
            }

            _context.AppLogs.Remove(appLog);
            await _context.SaveChangesAsync();

            return Ok(appLog);
        }

        private bool AppLogExists(long id)
        {
            return _context.AppLogs.Any(e => e.Id == id);
        }
    }
}
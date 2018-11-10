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
    /// Manage Device Data in server
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public DevicesController(PDJayaDB context)
        {
            _context = context;
        }
        [HttpGet("[action]")]
        public ActionResult GetDeviceByNo([FromQuery]string DeviceNo)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.Devices
                            where x.DeviceNo == DeviceNo
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
        /// <summary>
        /// Update device status
        /// </summary>
        /// <param name="DeviceNo"></param>
        /// <param name="DeviceStatus"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateDeviceStatus([FromQuery]string DeviceNo, [FromQuery]Models.DeviceStatus DeviceStatus)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var selDevice = from x in _context.Devices
                                where x.DeviceNo == DeviceNo
                                select x;
                foreach(var item in selDevice)
                {
                    item.Status = DeviceStatus;
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

        // GET: api/Devices
        [HttpGet]
        public IEnumerable<Device> GetDevices()
        {
            return _context.Devices;
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice([FromRoute] long id, [FromBody] Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.Id)
            {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        [HttpPost]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = device.Id }, device);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return Ok(device);
        }

        private bool DeviceExists(long id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
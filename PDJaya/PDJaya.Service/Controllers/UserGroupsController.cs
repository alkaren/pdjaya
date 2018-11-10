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
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class UserGroupsController : ControllerBase
    {
        private readonly PDJayaDB _context;

        public UserGroupsController(PDJayaDB context)
        {
            _context = context;
        }

        // GET: api/UserGroups
        [HttpGet]
        public IEnumerable<UserGroup> GetUserGroups()
        {
            return _context.UserGroups;
        }

        // GET: api/UserGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserGroup([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userGroup = await _context.UserGroups.FindAsync(id);

            if (userGroup == null)
            {
                return NotFound();
            }

            return Ok(userGroup);
        }

        // PUT: api/UserGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserGroup([FromRoute] long id, [FromBody] UserGroup userGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(userGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGroupExists(id))
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

        // POST: api/UserGroups
        [HttpPost]
        public async Task<IActionResult> PostUserGroup([FromBody] UserGroup userGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserGroup", new { id = userGroup.Id }, userGroup);
        }

        // DELETE: api/UserGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGroup([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();

            return Ok(userGroup);
        }

        private bool UserGroupExists(long id)
        {
            return _context.UserGroups.Any(e => e.Id == id);
        }
    }
}
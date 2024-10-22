using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoAuthAPI.Models;

namespace DemoAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTokensController : ControllerBase
    {
        private readonly DemoAuthContext _context;

        public UserTokensController(DemoAuthContext context)
        {
            _context = context;
        }

        // GET: api/UserTokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserToken>>> GetUserTokens()
        {
          if (_context.UserTokens == null)
          {
              return NotFound();
          }
            return await _context.UserTokens.ToListAsync();
        }

        // GET: api/UserTokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserToken>> GetUserToken(int id)
        {
          if (_context.UserTokens == null)
          {
              return NotFound();
          }
            var userToken = await _context.UserTokens.FindAsync(id);

            if (userToken == null)
            {
                return NotFound();
            }

            return userToken;
        }

        // PUT: api/UserTokens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserToken(int id, UserToken userToken)
        {
            if (id != userToken.UserTokenId)
            {
                return BadRequest();
            }

            _context.Entry(userToken).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTokenExists(id))
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

        // POST: api/UserTokens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserToken>> PostUserToken(UserToken userToken)
        {
          if (_context.UserTokens == null)
          {
              return Problem("Entity set 'DemoAuthContext.UserTokens'  is null.");
          }
            _context.UserTokens.Add(userToken);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserToken", new { id = userToken.UserTokenId }, userToken);
        }

        // DELETE: api/UserTokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserToken(int id)
        {
            if (_context.UserTokens == null)
            {
                return NotFound();
            }
            var userToken = await _context.UserTokens.FindAsync(id);
            if (userToken == null)
            {
                return NotFound();
            }

            _context.UserTokens.Remove(userToken);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTokenExists(int id)
        {
            return (_context.UserTokens?.Any(e => e.UserTokenId == id)).GetValueOrDefault();
        }
    }
}

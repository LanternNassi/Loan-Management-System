using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.UserX;
using Loan_Management_System.Utils;
using AutoMapper;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;


        public UsersController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery]string? keywords = null)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }

            var query = _context.Users.AsQueryable();

            if (keywords != null)
            {
                query = query.Where(c => c.Username.Contains(keywords));
            }

            return Ok(_mapper.Map<List<UserDto>>(await query.ToListAsync()));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserDto userUpdateDto)
        {
            if (id != userUpdateDto.Id)
            {
                return BadRequest();
            }

            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }

            // Update fields only if they are provided in the request
            dbUser.Username = !string.IsNullOrEmpty(userUpdateDto.Username) ? userUpdateDto.Username : dbUser.Username;
            dbUser.Email = !string.IsNullOrEmpty(userUpdateDto.Email) ? userUpdateDto.Email : dbUser.Email;
            dbUser.Role = !string.IsNullOrEmpty(userUpdateDto.Role) ? userUpdateDto.Role : dbUser.Role;


            _context.Entry(dbUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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



        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'DBContext.Users'  is null.");
          }

            user.PasswordHash = PasswordHasherUtility.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginSchema login)
        {
            if (login == null)
            {
                return BadRequest("Fill in the login info");
            }

            var user = _context.Users.Where(c => c.Username == login.Username).First();

            if (user == null)
            {
                return BadRequest("User doesnot exist");
            }

            if (PasswordHasherUtility.VerifyPassword(user.PasswordHash , login.password))
            {
                return Ok("Login successful");
            }

            return Unauthorized("Login Failed");

        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.SoftDelete(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

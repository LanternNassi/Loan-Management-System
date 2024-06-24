using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.ClientX;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ClientsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Clients
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients([FromQuery]string? keywords=null)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }

            var query = _context.Clients.Include(c => c.LoanApplications).AsQueryable();

            if (keywords != null)
            {
                query = query.Where(c => c.FirstName.Contains(keywords));
            }

            //return Ok(await query.ToListAsync());

            return Ok(_mapper.Map<List<ClientDto>>(await query.ToListAsync()));

        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ClientDto>> GetClient(Guid id)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClientDto>(client));
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> PutClient(Guid id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ClientDto>> PostClient(ClientDto client)
        {
          if (_context.Clients == null)
          {
              return Problem("Entity set 'DBContext.Clients'  is null.");
          }
            _context.Clients.Add(_mapper.Map<Client>(client));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, _mapper.Map<Client>(client));
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.SoftDelete(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(Guid id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

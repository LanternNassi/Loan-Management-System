using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.AccountX;
using AutoMapper;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public AccountsController(DBContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts([FromQuery]Guid? client=null)
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }

          var query = _context.Accounts.Include(c => c.Client).AsQueryable();


          if (client != null){
             query = query.Where(c => c.Client.Id == client);
          }


          return Ok(_mapper.Map<List<AccountDto>>(await query.ToListAsync()));
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AccountDto>(account));
        }

        
        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.SoftDelete(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

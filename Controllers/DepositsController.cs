using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.DepositX;
using AutoMapper;
using Loan_Management_System.Models.AccountX;

namespace Loan_Management_System.Controllers
{
    [Route("api/Account/[controller]")]
    [ApiController]
    public class DepositsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public DepositsController(DBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositDto>>> GetDeposits([FromQuery]Guid? client=null)
        {
          if (_context.Deposits == null)
          {
              return NotFound();
          }
            var query = _context.Deposits.AsQueryable();
            query = query.Include(c => c.Account).ThenInclude(c => c.Client);

            if (client != null)
            {
                query = query.Where(c => c.Account.Client.Id == client);
            }

            var meta_data = new
            {
                total = query.Sum(c => c.Amount),
                count = query.Count(),
            };

            return Ok(new
            {
                meta_data,
                data = _mapper.Map<List<DepositDto>>(await query.ToListAsync())
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deposit>> GetDeposit(Guid id)
        {
          if (_context.Deposits == null)
          {
              return NotFound();
          }
            var deposit = await _context.Deposits.FindAsync(id);

            if (deposit == null)
            {
                return NotFound();
            }

            return deposit;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeposit(Guid id, Deposit deposit)
        {
            if (id != deposit.Id)
            {
                return BadRequest();
            }

            _context.Entry(deposit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Deposit>> PostDeposit(DepositDto deposit)
        {
          if (_context.Deposits == null)
          {
              return Problem("Entity set 'DBContext.Deposits'  is null.");
          }
            _context.Deposits.Add(_mapper.Map<Deposit>(deposit));

            //Modify the Account
            Account? account = _context.Accounts.Find(deposit.AccountId);
            account.Balance += deposit.Amount;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeposit", new { id = deposit.Id }, deposit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposit(Guid id)
        {
            if (_context.Deposits == null)
            {
                return NotFound();
            }
            var deposit = await _context.Deposits.FindAsync(id);
            if (deposit == null)
            {
                return NotFound();
            }
            Account? account = _context.Accounts.Find(deposit.AccountId);
            account.Balance -= deposit.Amount;

            _context.SoftDelete(deposit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepositExists(Guid id)
        {
            return (_context.Deposits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

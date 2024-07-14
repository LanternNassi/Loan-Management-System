using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.WithdrawalX;
using AutoMapper;
using Loan_Management_System.Models.DepositX;
using Loan_Management_System.Models.AccountX;

namespace Loan_Management_System.Controllers
{
    [Route("api/Account/[controller]")]
    [ApiController]
    public class WithdrawalsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public WithdrawalsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WithdrawalDto>>> GetWithdrawals([FromQuery] Guid? client = null)
        {
            if (_context.Withdrawals == null)
            {
                return NotFound();
            }
            var query = _context.Withdrawals.AsQueryable();
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
                data = _mapper.Map<List<WithdrawalDto>>(await query.ToListAsync())
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WithdrawalDto>> GetWithdrawal(Guid id)
        {
          if (_context.Withdrawals == null)
          {
              return NotFound();
          }
            var withdrawal = await _context.Withdrawals.FindAsync(id);

            if (withdrawal == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WithdrawalDto>(withdrawal));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWithdrawal(Guid id, Withdrawal withdrawal)
        {
            if (id != withdrawal.Id)
            {
                return BadRequest();
            }

            _context.Entry(withdrawal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WithdrawalExists(id))
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
        public async Task<ActionResult<Withdrawal>> PostWithdrawal(WithdrawalDto withdrawal)
        {
          if (_context.Withdrawals == null)
          {
              return Problem("Entity set 'DBContext.Withdrawals'  is null.");
          }
            _context.Withdrawals.Add(_mapper.Map<Withdrawal>(withdrawal));

            //Modify the Account
            Account? account = _context.Accounts.Find(withdrawal.AccountId);

            if (withdrawal.Amount > account.Balance)
            {
                return BadRequest("Not enough balance on account");
            }
            account.Balance -= withdrawal.Amount;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWithdrawal", new { id = withdrawal.Id }, withdrawal);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWithdrawal(Guid id)
        {
            if (_context.Withdrawals == null)
            {
                return NotFound();
            }
            var withdrawal = await _context.Withdrawals.FindAsync(id);
            if (withdrawal == null)
            {
                return NotFound();
            }

            //Modify the Account
            Account? account = _context.Accounts.Find(withdrawal.AccountId);
            account.Balance += withdrawal.Amount;

            _context.SoftDelete(withdrawal);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WithdrawalExists(Guid id)
        {
            return (_context.Withdrawals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

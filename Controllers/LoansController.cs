using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.LoanX;
using AutoMapper;
using Loan_Management_System.Models.LoanApplicationX;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public LoansController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans(
            [FromQuery] string? status = null,
            [FromQuery] Guid? client = null
        )
        {
          if (_context.Loans == null)
          {
              return NotFound();
          }

            var query = _context.Loans.AsQueryable();
            query = query.Include(c => c.LoanApplication).ThenInclude(c => c.Client);

            if (status != null)
            {
                query = query.Where(c => c.Status== status);
            }

            if (client != null)
            {
                query = query.Where(c => c.LoanApplication.Client.Id == client);
            }

            return Ok(_mapper.Map<List<LoanDto>>(await query.ToListAsync()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoan(Guid id)
        {
          if (_context.Loans == null)
          {
              return NotFound();
          }
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LoanDto>(loan));
        }

        // PUT: api/Loans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(Guid id, Loan loan)
        {
            if (id != loan.Id)
            {
                return BadRequest();
            }

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
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

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostLoan(LoanDto loand)
        {
          if (_context.Loans == null)
          {
              return Problem("Entity set 'DBContext.Loans'  is null.");
          }
            _context.Loans.Add(_mapper.Map<Loan>(loand));
            await _context.SaveChangesAsync();

            return Ok();

        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(Guid id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.SoftDelete(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(Guid id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

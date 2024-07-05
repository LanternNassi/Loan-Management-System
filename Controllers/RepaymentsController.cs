using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.RepaymentsX;
using AutoMapper;
using Loan_Management_System.Models.RepaymentScheduleX;
using Loan_Management_System.Models.LoanX;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepaymentsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public RepaymentsController(DBContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Repayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepaymentDto>>> GetRepayments()
        {
          if (_context.Repayments == null)
          {
              return NotFound();
          }
            return _mapper.Map<List<RepaymentDto>>(await _context.Repayments.ToListAsync());
        }

        // GET: api/Repayments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepaymentDto>> GetRepayment(Guid id)
        {
          if (_context.Repayments == null)
          {
              return NotFound();
          }
            var repayment = await _context.Repayments.FindAsync(id);

            if (repayment == null)
            {
                return NotFound();
            }

            return _mapper.Map<RepaymentDto>(repayment);
        }

        // PUT: api/Repayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepayment(Guid id, RepaymentDto repayment)
        {
            if (id != repayment.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<Repayment>(repayment)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepaymentExists(id))
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

        // POST: api/Repayments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RepaymentDto>> PostRepayment(RepaymentDto repayment)
        {
              if (_context.Repayments == null)
              {
                  return Problem("Entity set 'DBContext.Repayments'  is null.");
              }

            RepaymentSchedule? schedule = _context.RepaymentSchedules.Find(repayment.Schedule);
            Loan? loan = _context.Loans.Find(schedule?.loanId);

            if (schedule != null && loan != null)
            {

                _context.Repayments.Add(_mapper.Map<Repayment>(repayment));


                // Updating the Repayment schedule
                schedule.PaidAmount += repayment.Amount;

                if (schedule.PaidAmount >= schedule.RepaymentAmount)
                {
                    schedule.Status = "Paid";
                }


                //Updating the Loan
                var loan_balance = (loan.OutStandingBalance - repayment.Amount);

                if (loan_balance <= 0)
                {
                    loan.Status = "Repaid";
                }
                loan.OutStandingBalance = loan_balance;


            }

            await _context.SaveChangesAsync();


            return Ok("Payment added successfully");
        }

        // DELETE: api/Repayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepayment(Guid id)
        {
            if (_context.Repayments == null)
            {
                return NotFound();
            }
            var repayment = await _context.Repayments.FindAsync(id);
            if (repayment == null)
            {
                return NotFound();
            }

            _context.Repayments.Remove(repayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RepaymentExists(Guid id)
        {
            return (_context.Repayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

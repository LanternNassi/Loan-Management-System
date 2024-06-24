using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.LoanApplicationX;
using AutoMapper;
using Loan_Management_System.Models.LoanX;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public LoanApplicationsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LoanApplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanApplicationDto>>> GetLoanApplications([FromQuery] LoanApplicationQuery? applicationquery )
        {
          if (_context.LoanApplications == null)
          {
              return NotFound();
          }

            var query = _context.LoanApplications.AsQueryable();

            if (!string.IsNullOrEmpty(applicationquery.ClientId.ToString()))
            {
                query = query.Where(c => c.ClientId== applicationquery.ClientId);
            }

            if (!string.IsNullOrEmpty(applicationquery.Status))
            {
                query = query.Where(c => c.Status == applicationquery.Status);
            }

            if (!string.IsNullOrEmpty(applicationquery.Approved_by.ToString()))
            {
                query = query.Where(c => c.Approved_by == applicationquery.Approved_by);
            }

            return Ok(_mapper.Map<List<LoanApplicationDto>>(await query.ToListAsync()));

        }

        // GET: api/LoanApplications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanApplicationDto>> GetLoanApplication(Guid id)
        {
          if (_context.LoanApplications == null)
          {
              return NotFound();
          }
            var loanApplication = await _context.LoanApplications.FindAsync(id);

            if (loanApplication == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LoanApplicationDto>(loanApplication));
        }

        // PUT: api/LoanApplications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanApplication(Guid id, LoanCreation loanApplication)
        {
            if (id != loanApplication.Id)
            {
                return BadRequest();
            }

            var initial_loanApplication = await _context.LoanApplications.FindAsync(id);

            if (loanApplication.Status == "Approved" && (initial_loanApplication.Status == "Pending"))
            {
                //Create Loan automatically

                Loan new_loan = new Loan();
                new_loan.Application = id;
                new_loan.LoanAmount = initial_loanApplication.LoanAmount;
                new_loan.StartDate = loanApplication.StartDate;
                new_loan.EndDate = loanApplication.EndDate;
                new_loan.InterestRate = loanApplication.InterestRate;
                new_loan.Status = "Active";

                decimal? interest = initial_loanApplication.LoanAmount * (loanApplication.InterestRate/100);
                new_loan.OutStandingBalance = interest.HasValue ? interest.Value + initial_loanApplication.LoanAmount : initial_loanApplication.LoanAmount;

                _context.Loans.Add(new_loan);

                loanApplication.Approved_Date = DateTime.Now;

            }

            var new_loanApplication = _mapper.Map<LoanApplication>(loanApplication);

            new_loanApplication.AddedAt = initial_loanApplication.AddedAt;

            _context.Entry(initial_loanApplication).CurrentValues.SetValues(new_loanApplication);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanApplicationExists(id))
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

        // POST: api/LoanApplications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanApplication>> PostLoanApplication(LoanApplicationDto loanApplication)
        {
          if (_context.LoanApplications == null)
          {
              return Problem("Entity set 'DBContext.LoanApplications'  is null.");
          }
            _context.LoanApplications.Add(_mapper.Map<LoanApplication>(loanApplication));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoanApplication", new { id = loanApplication.Id }, _mapper.Map<LoanApplication>(loanApplication));
        }

        // DELETE: api/LoanApplications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanApplication(Guid id)
        {
            if (_context.LoanApplications == null)
            {
                return NotFound();
            }
            var loanApplication = await _context.LoanApplications.FindAsync(id);
            if (loanApplication == null)
            {
                return NotFound();
            }

            _context.SoftDelete(loanApplication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]

        private bool LoanApplicationExists(Guid id)
        {
            return (_context.LoanApplications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

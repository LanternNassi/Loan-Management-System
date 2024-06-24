using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.LoanDisbursmentX;
using AutoMapper;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanDisbursmentsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public LoanDisbursmentsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LoanDisbursments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDisbursmentDto>>> GetLoanDisbursments([FromQuery]Guid? LoanId=null , [FromQuery]Guid? Disbursed_by=null)
        {
          if (_context.LoanDisbursments == null)
          {
              return NotFound();
          }

            var query = _context.LoanDisbursments.AsQueryable();

            if (LoanId != null)
            {
                query = query.Where(c => c.loanId == LoanId);
            }

            if (Disbursed_by != null)
            {
                query = query.Where(c => c.DisbursedBy == Disbursed_by);
            }

            return Ok(_mapper.Map<List<LoanDisbursmentDto>>(await query.ToListAsync()));
        }

        // GET: api/LoanDisbursments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDisbursmentDto>> GetLoanDisbursment(Guid id)
        {
          if (_context.LoanDisbursments == null)
          {
              return NotFound();
          }
            var loanDisbursment = await _context.LoanDisbursments.FindAsync(id);

            if (loanDisbursment == null)
            {
                return NotFound();
            }

            return _mapper.Map<LoanDisbursmentDto>(loanDisbursment);
        }

        //// PUT: api/LoanDisbursments/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLoanDisbursment(Guid id, LoanDisbursmentDto loanDisbursment)
        //{
        //    if (id != loanDisbursment.Id)
        //    {
        //        return BadRequest();
        //    }

        //    LoanDisbursmentDto d = _mapper.Map<LoanDisbursmentDto>()

        //    _context.Entry(_mapper.Map<LoanDisbursment>()).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LoanDisbursmentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/LoanDisbursments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanDisbursment>> PostLoanDisbursment(LoanDisbursmentDto loanDisbursment)
        {
          if (_context.LoanDisbursments == null)
          {
              return Problem("Entity set 'DBContext.LoanDisbursments'  is null.");
          }
            _context.LoanDisbursments.Add(_mapper.Map<LoanDisbursment>(loanDisbursment));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoanDisbursment", new { id = loanDisbursment.Id }, loanDisbursment);
        }

        // DELETE: api/LoanDisbursments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanDisbursment(Guid id)
        {
            if (_context.LoanDisbursments == null)
            {
                return NotFound();
            }
            var loanDisbursment = await _context.LoanDisbursments.FindAsync(id);
            if (loanDisbursment == null)
            {
                return NotFound();
            }

            _context.SoftDelete(loanDisbursment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanDisbursmentExists(Guid id)
        {
            return (_context.LoanDisbursments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

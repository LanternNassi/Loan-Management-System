﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_Management_System.Models;
using Loan_Management_System.Models.RepaymentScheduleX;
using AutoMapper;
using System.Net;

namespace Loan_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepaymentSchedulesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public RepaymentSchedulesController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/RepaymentSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepaymentScheduleDto>>> GetRepaymentSchedules([FromQuery]Guid? loanId = null)
        {
          if (_context.RepaymentSchedules == null)
          {
              return NotFound();
          }

            var query = _context.RepaymentSchedules.AsQueryable();

          if (loanId != null)
            {
                query = query.Where(c => c.loanId == loanId);
            }

            return Ok(_mapper.Map<List<RepaymentScheduleDto>>(await query.ToListAsync()));
        }

        // GET: api/RepaymentSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepaymentScheduleDto>> GetRepaymentSchedule(Guid id)
        {
          if (_context.RepaymentSchedules == null)
          {
              return NotFound();
          }
            var repaymentSchedule = await _context.RepaymentSchedules.FindAsync(id);

            if (repaymentSchedule == null)
            {
                return NotFound();
            }

            return _mapper.Map<RepaymentScheduleDto>(repaymentSchedule);
        }

        // PUT: api/RepaymentSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepaymentSchedule(Guid id, RepaymentSchedule repaymentSchedule)
        {
            if (id != repaymentSchedule.id)
            {
                return BadRequest();
            }

            _context.Entry(repaymentSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepaymentScheduleExists(id))
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

        [HttpPost("UpdatePayment")]
        public async Task<IActionResult> UpdatePayment(UpdatePaymentDto payment_info)
        {
            if (_context.RepaymentSchedules == null)
            {
                return BadRequest();
            }



            var scheduled_payment = await _context.RepaymentSchedules.FindAsync(payment_info.id);

            scheduled_payment.Status = payment_info.Status;

            await _context.SaveChangesAsync();

            return Ok();

            //if (scheduled_payment.Status == "Paid")
            //{
            //    return BadRequest("The scheduled payment is already paid");
            //}

            //if (scheduled_payment.Status == "Missed")
            //{
            //    return BadRequest("The scheduled payment was missed");
            //}

            //if ((scheduled_payment.RepaymentAmount + payment_info.Amount) > scheduled_payment.RepaymentAmount)
            //{
            //    return BadRequest("The amount paid cant be more than the scheduled repayment");
            //}

            //if (DateTime.Now > scheduled_payment.RepaymentDate)
            //{
            //    return BadRequest("The scheduled time for the payment has passed");
            //}

            //var new_amount = scheduled_payment.RepaymentAmount + payment_info.Amount;

            //scheduled_payment.RepaymentAmount = new_amount;

            //if (scheduled_payment)
        }

        // POST: api/RepaymentSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostRepaymentSchedule(RepaymentScheduleDto repaymentSchedule)
        {
          if (_context.RepaymentSchedules == null)
          {
              return Problem("Entity set 'DBContext.RepaymentSchedules'  is null.");
          }
            _context.RepaymentSchedules.Add(_mapper.Map<RepaymentSchedule>(repaymentSchedule));
            await _context.SaveChangesAsync();

            return Ok();

        }

        // DELETE: api/RepaymentSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepaymentSchedule(Guid id)
        {
            if (_context.RepaymentSchedules == null)
            {
                return NotFound();
            }
            var repaymentSchedule = await _context.RepaymentSchedules.FindAsync(id);
            if (repaymentSchedule == null)
            {
                return NotFound();
            }

            _context.RepaymentSchedules.Remove(repaymentSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RepaymentScheduleExists(Guid id)
        {
            return (_context.RepaymentSchedules?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

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
using Microsoft.AspNetCore.Authorization;
using System.Text;

using Loan_Management_System.Models.RepaymentsX;
using Loan_Management_System.Models.DepositX;
using Loan_Management_System.Models.LoanApplicationX;

namespace Loan_Management_System.Controllers
{

    public class PaymentsMetric {
        public decimal amount { get; set; } 
        public string firstName { get; set; }
        public string otherNames { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(DBContext context, IMapper mapper , IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("/api/metrics")]
        public async Task<IActionResult> GetMetrics(){

            var users = new {
                admin =  _context.Users.Where(c => c.Role == "admin").Count(),
                normal = _context.Users.Where(c => c.Role == "normal").Count(),
            };

            var clients = new {
                active_clients = _context.Clients.Count(),
                deleted_clients = 0,
            };

            var applications = new {
                approved = _context.LoanApplications.Where(c => c.Status == "Approved").Count(),
                pending = _context.LoanApplications.Where(c => c.Status == "Pending").Count(),
                rejected = _context.LoanApplications.Where(c => c.Status == "Rejected").Count(),
                pending_applications = _mapper.Map<List<LoanApplicationDto>>(await _context.LoanApplications.Where(c => c.Status == "Pending").Include(c => c.Client).OrderByDescending(e => e.AddedAt).Take(4).ToListAsync()),

            };

            var loans = new {
                active = _context.Loans.Where(c => c.Status == "Active").Count(),
                repaid = _context.Loans.Where(c => c.Status == "Repaid").Count(),
                defaulted = _context.Loans.Where(c => c.Status == "Defaulted").Count()
            };

            var payments = _context.Repayments
                .Include(c => c.RepaymentSchedule)
                    .ThenInclude(c => c.Loan)
                        .ThenInclude(c => c.LoanApplication)
                            .ThenInclude(c => c.Client)
                .OrderByDescending(e => e.AddedAt).Take(4).ToList();

            List<PaymentsMetric> processed_payments = new List<PaymentsMetric>();
            foreach (var payment in payments){
                processed_payments.Add(
                    new PaymentsMetric{
                        firstName = payment.RepaymentSchedule.Loan.LoanApplication.Client.FirstName,
                        otherNames = payment.RepaymentSchedule.Loan.LoanApplication.Client.OtherNames,
                        amount = payment.Amount
                    }
                );
            }

            var deposits = _mapper.Map<List<DepositDto>>(_context.Deposits.Include(c => c.Account).ThenInclude(c => c.Client).OrderByDescending(e => e.AddedAt).Take(4).ToList());

            return Ok(new {
                users,
                clients,
                applications,
                processed_payments,
                deposits,
                loans
            });
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery]string? keywords = null , [FromHeader]string? Authorization=null)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }

            //var claims = User.Claims;

            //StringBuilder claimList = new StringBuilder();
            //foreach (var claim in claims)
            //{
            //    claimList.AppendLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            //}


            var query = _context.Users.AsQueryable();

            if (keywords != null)
            {
                query = query.Where(c => c.Username.Contains(keywords));
            }

            //return Ok(claimList.ToString());

            return Ok(_mapper.Map<List<UserDto>>(await query.ToListAsync()));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
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
        [Authorize(Policy = "Admin")]
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
        [AllowAnonymous]
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

        [AllowAnonymous]
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
                var token = TokenGenerator.GenerateToken(_config , user);
                return Ok(new
                {
                    Username = login.Username,
                    id = user.Id,
                    role = user.Role,
                    token = token
                });
            }

            return Unauthorized("Login Failed");

        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
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

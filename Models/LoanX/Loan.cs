

using Loan_Management_System.Models.ClientX;
using Loan_Management_System.Models.LoanApplicationX;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loan_Management_System.Models.LoanX
{
    public class Loan : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid Application { get; set; }
        public decimal LoanAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? InterestRate { get; set; }
        public string Status { get; set; }
        public decimal OutStandingBalance { get; set; }

        [ForeignKey("Application")]
        public virtual LoanApplication LoanApplication { get; set; }

    }

    public class LoanDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid Application { get; set; }
        public decimal LoanAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? InterestRate { get; set; }
        public string Status { get; set; } // E.g., "Active", "Repaid", "Defaulted"
        public decimal OutStandingBalance { get; set; }
        public LoanApplicationDto? LoanApplication { get; set; }
    }

    public class LoanCreation :LoanApplicationDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? InterestRate { get; set; }

    }

}

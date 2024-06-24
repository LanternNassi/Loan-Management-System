
using Loan_Management_System.Models.ClientX;
using Loan_Management_System.Models.LoanApplicationX;
using System.ComponentModel.DataAnnotations.Schema;
using Loan_Management_System.Models.LoanX;

namespace Loan_Management_System.Models.RepaymentScheduleX
{
    public class RepaymentSchedule : GeneralFields
    {
        public Guid id { get; set; }
        public Guid loanId { get; set; }
        public DateTime RepaymentDate { get; set; }
        public decimal RepaymentAmount { get; set; }
        public string Status { get; set; }

        [ForeignKey("loanId")]
        public virtual Loan Loan { get; set; }
    }


    public class RepaymentScheduleDto : GeneralFields
    {
        public Guid id { get; set; }
        public Guid loanId { get; set; }
        public DateTime RepaymentDate { get; set; }
        public decimal RepaymentAmount { get; set; }
        public string Status { get; set; } // Pending , Missed , Paid
    }

    public class UpdatePaymentDto
    {
        public Guid? id { get; set; }


        public string? Status { get; set; }
    }
}

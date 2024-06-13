

using Loan_Management_System.Models.LoanApplicationX;
using Loan_Management_System.Models.RepaymentScheduleX;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loan_Management_System.Models.RepaymentsX
{
    public class Repayment : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid Schedule { get; set; }
        public decimal Amount { get; set; }
        public string? MoreInfo { get; set; }

        [ForeignKey("Schedule")]
        public virtual RepaymentSchedule RepaymentSchedule { get; set; }

    }

    public class RepaymentDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid Schedule { get; set; }
        public decimal Amount { get; set; }
        public string? MoreInfo { get; set; }

    }
}

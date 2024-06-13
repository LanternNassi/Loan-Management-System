
using Loan_Management_System.Models.ClientX;
using Loan_Management_System.Models.LoanApplicationX;
using System.ComponentModel.DataAnnotations.Schema;
using Loan_Management_System.Models.LoanX;

namespace Loan_Management_System.Models.RepaymentScheduleX
{
    public class RepaymentSchedule : GeneralFields
    {
        public Guid id { get; set; }
        public Guid Loan { get; set; }
        public DateTime RepaymentDate { get; set; }
        public decimal RepaymentAmout { get; set; }
        public string Status { get; set; }

        [ForeignKey("Loan")]
        public virtual Loan loan { get; set; }
    }


    public class RepaymentScheduleDto : GeneralFields
    {
        public Guid id { get; set; }
        public Guid Loan { get; set; }
        public DateTime RepaymentDate { get; set; }
        public decimal RepaymentAmout { get; set; }
        public string Status { get; set; }
    }
}

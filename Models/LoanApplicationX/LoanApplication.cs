
using Loan_Management_System.Models.UserX;
using Loan_Management_System.Models.ClientX;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loan_Management_System.Models.LoanApplicationX
{
    public class LoanApplication : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal LoanAmount { get; set; }
        public string Status { get; set; }
        public Guid? Approved_by { get; set; }
        public DateTime? Approved_Date { get; set; }
        public string? RejectionReason { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("Approved_by")]
        public virtual User User { get; set; }
    }


    public class LoanApplicationDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal LoanAmount { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected"
        public Guid? Approved_by { get; set; }
        public DateTime? Approved_Date { get; set; }
        public string? RejectionReason { get; set; }

    }

    public class LoanApplicationQuery
    {
        public Guid? ClientId { get; set; }
        public string? Status { get; set; }
        public Guid? Approved_by { get; set; }
    }
}

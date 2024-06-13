
using Loan_Management_System.Models.UserX;
using Loan_Management_System.Models.ClientX;
using System.ComponentModel.DataAnnotations.Schema;
using Loan_Management_System.Models.LoanApplicationX;
using Loan_Management_System.Models.LoanX;

namespace Loan_Management_System.Models.LoanDisbursmentX
{
    public class LoanDisbursment : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid Loan { get; set; }
        public DateTime DisbursmentDate { get; set; }
        public decimal? DisbursmentAmount { get; set;}
        public string? MoreInfo { get; set; }
        public Guid? DisbursedBy { get; set; }

        [ForeignKey("Loan")]
        public virtual Loan loan { get; set; }

        [ForeignKey("DisbursedBy")]
        public virtual User User { get; set; }

    }

    public class LoanDisbursmentDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid Loan { get; set; }
        public DateTime? DisbursmentDate { get; set; }
        public decimal DisbursmentAmount { get; set; }
        public string? MoreInfo { get; set; }
        public Guid? DisbursedBy { get; set; }
    }


}

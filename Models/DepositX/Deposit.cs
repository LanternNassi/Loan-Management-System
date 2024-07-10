
using Loan_Management_System.Models.AccountX;
using System.ComponentModel.DataAnnotations.Schema;


namespace Loan_Management_System.Models.DepositX
{
    public class Deposit : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; } 
        public decimal Amount { get; set; }
        public string? Description { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }


    public class DepositDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public AccountDto? Account { get; set; }
    }


}

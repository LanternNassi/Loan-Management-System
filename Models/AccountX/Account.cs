

using Loan_Management_System.Models.ClientX;
using System.ComponentModel.DataAnnotations.Schema;


namespace Loan_Management_System.Models.AccountX
{
    public class Account : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }
  
    }

    public class AccountDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }
        public ClientDto? Client { get; set; }
    }
}

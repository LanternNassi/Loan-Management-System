using System.Collections.Generic;
using Loan_Management_System.Models.LoanApplicationX;

namespace Loan_Management_System.Models.ClientX
{
    public class Client : GeneralFields
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string NIN { get; set; }
        public virtual ICollection<LoanApplication> LoanApplications { get; set; }


    }

    public class ClientDto : GeneralFields
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string NIN { get; set; }

    }
}

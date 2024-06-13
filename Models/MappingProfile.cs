
using AutoMapper;
using Loan_Management_System.Models.UserX;
using Loan_Management_System.Models.ClientX;
using Loan_Management_System.Models.LoanApplicationX;
using Loan_Management_System.Models.LoanDisbursmentX;
using Loan_Management_System.Models.LoanX;
using Loan_Management_System.Models.RepaymentScheduleX;
using Loan_Management_System.Models.RepaymentsX;

namespace Loan_Management_System.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 

            CreateMap<User , UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto , Client>();

            CreateMap<LoanApplication, LoanApplicationDto>();
            CreateMap<LoanApplicationDto, LoanApplication>();

            CreateMap<LoanDisbursment, LoanDisbursmentDto>();
            CreateMap<LoanDisbursmentDto, LoanDisbursment>();

            CreateMap<Loan, LoanDto>();
            CreateMap<LoanDto, Loan>();

            CreateMap<RepaymentSchedule, RepaymentScheduleDto>();
            CreateMap<RepaymentScheduleDto, RepaymentSchedule>();

            CreateMap<Repayment, RepaymentDto>();
            CreateMap<RepaymentDto, Repayment>();
        
        }

    }
}

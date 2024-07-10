
using AutoMapper;
using Loan_Management_System.Models.UserX;
using Loan_Management_System.Models.ClientX;
using Loan_Management_System.Models.LoanApplicationX;
using Loan_Management_System.Models.LoanDisbursmentX;
using Loan_Management_System.Models.LoanX;
using Loan_Management_System.Models.RepaymentScheduleX;
using Loan_Management_System.Models.RepaymentsX;
using Loan_Management_System.Models.AccountX;
using Loan_Management_System.Models.DepositX;
using Loan_Management_System.Models.WithdrawalX;

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
            CreateMap<LoanCreation, LoanApplication>();

            CreateMap<LoanDisbursment, LoanDisbursmentDto>();
            CreateMap<LoanDisbursmentDto, LoanDisbursment>();

            CreateMap<Loan, LoanDto>();
            CreateMap<LoanDto, Loan>();

            CreateMap<RepaymentSchedule, RepaymentScheduleDto>();
            CreateMap<RepaymentScheduleDto, RepaymentSchedule>();

            CreateMap<Repayment, RepaymentDto>();
            CreateMap<RepaymentDto, Repayment>();

            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<Deposit, DepositDto>();
            CreateMap<DepositDto, Deposit>();

            CreateMap<Withdrawal, WithdrawalDto>();
            CreateMap<WithdrawalDto, Withdrawal>();
        
        }

    }
}

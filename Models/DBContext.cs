using Loan_Management_System.Models.UserX;
using Loan_Management_System.Models.ClientX;
using Loan_Management_System.Models.LoanApplicationX;
using Loan_Management_System.Models.LoanDisbursmentX;
using Loan_Management_System.Models.LoanX;
using Loan_Management_System.Models.RepaymentScheduleX;
using Loan_Management_System.Models.RepaymentsX;


using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Loan_Management_System.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<LoanDisbursment> LoanDisbursments { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
        public DbSet<Repayment> Repayments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Client>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<LoanApplication>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<LoanDisbursment>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Loan>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<RepaymentSchedule>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Repayment>().HasQueryFilter(c => !c.DeletedAt.HasValue);


            base.OnModelCreating(modelBuilder);

        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is User
                    || e.Entity is Client
                    || e.Entity is LoanApplication
                    || e.Entity is LoanDisbursment
                    || e.Entity is Loan
                    || e.Entity is RepaymentSchedule
                    || e.Entity is Repayment
                    )
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("AddedAt").CurrentValue = DateTime.UtcNow;
                }
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        public void SoftDelete<T>(T entity) where T : class
        {
            var entry = Entry(entity);
            entry.Property("DeletedAt").CurrentValue = DateTime.UtcNow;
            entry.State = EntityState.Modified;
        }


    }
}

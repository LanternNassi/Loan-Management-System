using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Management_System.Migrations
{
    public partial class change_to_loan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepaymentSchedules_Loans_Loan",
                table: "RepaymentSchedules");

            migrationBuilder.RenameColumn(
                name: "Loan",
                table: "RepaymentSchedules",
                newName: "loan");

            migrationBuilder.RenameIndex(
                name: "IX_RepaymentSchedules_Loan",
                table: "RepaymentSchedules",
                newName: "IX_RepaymentSchedules_loan");

            migrationBuilder.AddForeignKey(
                name: "FK_RepaymentSchedules_Loans_loan",
                table: "RepaymentSchedules",
                column: "loan",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepaymentSchedules_Loans_loan",
                table: "RepaymentSchedules");

            migrationBuilder.RenameColumn(
                name: "loan",
                table: "RepaymentSchedules",
                newName: "Loan");

            migrationBuilder.RenameIndex(
                name: "IX_RepaymentSchedules_loan",
                table: "RepaymentSchedules",
                newName: "IX_RepaymentSchedules_Loan");

            migrationBuilder.AddForeignKey(
                name: "FK_RepaymentSchedules_Loans_Loan",
                table: "RepaymentSchedules",
                column: "Loan",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

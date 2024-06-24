using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Management_System.Migrations
{
    public partial class change_to_loanId_sure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepaymentSchedules_Loans_loan",
                table: "RepaymentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_RepaymentSchedules_loan",
                table: "RepaymentSchedules");

            migrationBuilder.DropColumn(
                name: "loan",
                table: "RepaymentSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_RepaymentSchedules_loanId",
                table: "RepaymentSchedules",
                column: "loanId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepaymentSchedules_Loans_loanId",
                table: "RepaymentSchedules",
                column: "loanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepaymentSchedules_Loans_loanId",
                table: "RepaymentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_RepaymentSchedules_loanId",
                table: "RepaymentSchedules");

            migrationBuilder.AddColumn<Guid>(
                name: "loan",
                table: "RepaymentSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RepaymentSchedules_loan",
                table: "RepaymentSchedules",
                column: "loan");

            migrationBuilder.AddForeignKey(
                name: "FK_RepaymentSchedules_Loans_loan",
                table: "RepaymentSchedules",
                column: "loan",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

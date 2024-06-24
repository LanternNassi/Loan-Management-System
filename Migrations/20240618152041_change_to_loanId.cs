using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Management_System.Migrations
{
    public partial class change_to_loanId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepaymentAmout",
                table: "RepaymentSchedules",
                newName: "RepaymentAmount");

            migrationBuilder.AddColumn<Guid>(
                name: "loanId",
                table: "RepaymentSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loanId",
                table: "RepaymentSchedules");

            migrationBuilder.RenameColumn(
                name: "RepaymentAmount",
                table: "RepaymentSchedules",
                newName: "RepaymentAmout");
        }
    }
}

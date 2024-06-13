using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Management_System.Migrations
{
    public partial class Loans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Client",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Loans_Client",
                table: "Loans",
                column: "Client");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Clients_Client",
                table: "Loans",
                column: "Client",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Clients_Client",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_Client",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "Loans");
        }
    }
}

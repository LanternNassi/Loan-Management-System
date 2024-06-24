using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Management_System.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approved_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approved_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanApplications_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanApplications_Users_Approved_by",
                        column: x => x.Approved_by,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Application = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutStandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_LoanApplications_Application",
                        column: x => x.Application,
                        principalTable: "LoanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanDisbursments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    loanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisbursmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MoreInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisbursedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanDisbursments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanDisbursments_Loans_loanId",
                        column: x => x.loanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanDisbursments_Users_DisbursedBy",
                        column: x => x.DisbursedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepaymentSchedules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Loan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepaymentAmout = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepaymentSchedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_RepaymentSchedules_Loans_Loan",
                        column: x => x.Loan,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Schedule = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoreInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repayments_RepaymentSchedules_Schedule",
                        column: x => x.Schedule,
                        principalTable: "RepaymentSchedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_Approved_by",
                table: "LoanApplications",
                column: "Approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_ClientId",
                table: "LoanApplications",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursments_DisbursedBy",
                table: "LoanDisbursments",
                column: "DisbursedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursments_loanId",
                table: "LoanDisbursments",
                column: "loanId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_Application",
                table: "Loans",
                column: "Application");

            migrationBuilder.CreateIndex(
                name: "IX_Repayments_Schedule",
                table: "Repayments",
                column: "Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_RepaymentSchedules_Loan",
                table: "RepaymentSchedules",
                column: "Loan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanDisbursments");

            migrationBuilder.DropTable(
                name: "Repayments");

            migrationBuilder.DropTable(
                name: "RepaymentSchedules");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "LoanApplications");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

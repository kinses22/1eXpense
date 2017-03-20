using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExpensesApplication.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    CCExpenseType = table.Column<string>(nullable: true),
                    Chargeable = table.Column<bool>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    ExpenseDate = table.Column<DateTime>(nullable: false),
                    ExpenseStatus = table.Column<string>(nullable: true),
                    Expensetype = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    ProjectCode = table.Column<string>(nullable: true),
                    RejectionInfo = table.Column<string>(nullable: true),
                    SubmitDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense");
        }
    }
}

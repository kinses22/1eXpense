using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesApplication.Data.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Practice",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Practice",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Project",
                nullable: true);
        }
    }
}

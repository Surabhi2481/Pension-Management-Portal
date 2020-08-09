using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pension_Management_Portal.Migrations
{
    public partial class Initial01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pensionDetails",
                columns: table => new
                {
                    SerialNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Pan = table.Column<string>(nullable: true),
                    AadharNumber = table.Column<string>(nullable: true),
                    PensionType = table.Column<int>(nullable: false),
                    PensionAmount = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pensionDetails", x => x.SerialNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pensionDetails");
        }
    }
}

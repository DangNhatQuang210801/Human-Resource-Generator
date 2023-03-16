using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_Resource_Generator.Migrations
{
    public partial class AddGeneratorToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employee_Training",
                columns: table => new
                {
                    program_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    program_result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program_manager = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employee_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employee_department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    training_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_Training", x => x.program_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_Training");
        }
    }
}

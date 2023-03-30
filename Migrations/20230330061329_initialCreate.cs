using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_Resource_Generator.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreateTrainingProgramViewModel",
                columns: table => new
                {
                    program_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_program = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeViewModel",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeViewModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    program_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    program_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_program = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.program_id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgramViewModel",
                columns: table => new
                {
                    program_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    program_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_program = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProgramViewModel", x => x.program_id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTrainings",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    program_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTrainings", x => new { x.ID, x.program_id });
                    table.ForeignKey(
                        name: "FK_EmployeeTrainings_Employees_ID",
                        column: x => x.ID,
                        principalTable: "Employees",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_EmployeeTrainings_TrainingPrograms_program_id",
                        column: x => x.program_id,
                        principalTable: "TrainingPrograms",
                        principalColumn: "program_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTrainings_program_id",
                table: "EmployeeTrainings",
                column: "program_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreateTrainingProgramViewModel");

            migrationBuilder.DropTable(
                name: "EmployeeTrainings");

            migrationBuilder.DropTable(
                name: "EmployeeViewModel");

            migrationBuilder.DropTable(
                name: "TrainingProgramViewModel");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");
        }
    }
}

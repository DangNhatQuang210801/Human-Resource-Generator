using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_Resource_Generator.Migrations
{
    public partial class ManyToManyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_employee_Training",
                table: "employee_Training");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "employee_Training");

            migrationBuilder.DropColumn(
                name: "employee_department",
                table: "employee_Training");

            migrationBuilder.DropColumn(
                name: "employee_name",
                table: "employee_Training");

            migrationBuilder.DropColumn(
                name: "program_name",
                table: "employee_Training");

            migrationBuilder.DropColumn(
                name: "program_result",
                table: "employee_Training");

            migrationBuilder.DropColumn(
                name: "training_date",
                table: "employee_Training");

            migrationBuilder.RenameTable(
                name: "employee_Training",
                newName: "employees_Training");

            migrationBuilder.AlterColumn<int>(
                name: "program_id",
                table: "employees_Training",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employees_Training",
                table: "employees_Training",
                column: "employee_id");

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employee_department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "training_Program",
                columns: table => new
                {
                    program_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    program_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    program_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_program = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training_Program", x => x.program_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employees_Training_program_id",
                table: "employees_Training",
                column: "program_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_Training_employee_employee_id",
                table: "employees_Training",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employees_Training_training_Program_program_id",
                table: "employees_Training",
                column: "program_id",
                principalTable: "training_Program",
                principalColumn: "program_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_Training_employee_employee_id",
                table: "employees_Training");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_Training_training_Program_program_id",
                table: "employees_Training");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "training_Program");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employees_Training",
                table: "employees_Training");

            migrationBuilder.DropIndex(
                name: "IX_employees_Training_program_id",
                table: "employees_Training");

            migrationBuilder.RenameTable(
                name: "employees_Training",
                newName: "employee_Training");

            migrationBuilder.AlterColumn<int>(
                name: "program_id",
                table: "employee_Training",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_birth",
                table: "employee_Training",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "employee_department",
                table: "employee_Training",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "employee_name",
                table: "employee_Training",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "program_name",
                table: "employee_Training",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "program_result",
                table: "employee_Training",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "training_date",
                table: "employee_Training",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_employee_Training",
                table: "employee_Training",
                column: "program_id");
        }
    }
}

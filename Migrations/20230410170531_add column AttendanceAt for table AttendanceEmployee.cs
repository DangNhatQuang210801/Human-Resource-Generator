using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_Resource_Generator.Migrations
{
    public partial class addcolumnAttendanceAtfortableAttendanceEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AttendanceAt",
                table: "AttendanceEmployees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceAt",
                table: "AttendanceEmployees");
        }
    }
}

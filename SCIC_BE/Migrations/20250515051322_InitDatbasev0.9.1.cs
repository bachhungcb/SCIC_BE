using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCIC_BE.Migrations
{
    /// <inheritdoc />
    public partial class InitDatbasev091 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceStudentModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAttended = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceStudentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceStudentModel_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceStudentModel_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceStudentModel_AttendanceId",
                table: "AttendanceStudentModel",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceStudentModel_StudentId",
                table: "AttendanceStudentModel",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceStudentModel");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCIC_BE.Migrations
{
    /// <inheritdoc />
    public partial class fixDatabasev02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LecturerInfos_Users_UserId",
                table: "LecturerInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfos_Users_UserId",
                table: "StudentInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentInfos",
                table: "StudentInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LecturerInfos",
                table: "LecturerInfos");

            migrationBuilder.RenameTable(
                name: "StudentInfos",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "LecturerInfos",
                newName: "Lecturer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecturer",
                table: "Lecturer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturer_Users_UserId",
                table: "Lecturer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Users_UserId",
                table: "Student",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturer_Users_UserId",
                table: "Lecturer");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Users_UserId",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecturer",
                table: "Lecturer");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "StudentInfos");

            migrationBuilder.RenameTable(
                name: "Lecturer",
                newName: "LecturerInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentInfos",
                table: "StudentInfos",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LecturerInfos",
                table: "LecturerInfos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerInfos_Users_UserId",
                table: "LecturerInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfos_Users_UserId",
                table: "StudentInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

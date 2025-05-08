using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCIC_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceImage",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FingerprintImage",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceImage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FingerprintImage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "Users");
        }
    }
}

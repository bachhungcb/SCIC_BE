using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCIC_BE.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabasev093 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAttended",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAttended",
                table: "Attendances");
        }
    }
}

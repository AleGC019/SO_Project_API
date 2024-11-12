using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RM_API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgeFieldDropped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAge",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserAge",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 18);
        }
    }
}

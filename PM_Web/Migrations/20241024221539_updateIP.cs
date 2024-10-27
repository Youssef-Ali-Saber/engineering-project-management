using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class updateIP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "InterfacePoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "InterfacePoints");
        }
    }
}

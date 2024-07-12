using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM01.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e3863a0-1ed1-4440-a57b-2f6699a6d594");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f596a376-825d-4cb5-9c92-14b0b9bd17fa");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Projects",
                newName: "Owners");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d9348fa-cc5b-4352-a3b6-d956f6b79765", "abad08bc-64e5-4987-a0b3-17c47575e1da", "Cordinator", "CORDINATOR" },
                    { "a429a0c7-a7fc-4cfe-8149-81df29bdf080", "21f85523-ab03-4b10-bb9a-c4ce33ac13eb", "TeamMember", "TEAMMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d9348fa-cc5b-4352-a3b6-d956f6b79765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a429a0c7-a7fc-4cfe-8149-81df29bdf080");

            migrationBuilder.RenameColumn(
                name: "Owners",
                table: "Projects",
                newName: "Owner");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e3863a0-1ed1-4440-a57b-2f6699a6d594", null, "TeamMember", "TEAMMEMBER" },
                    { "f596a376-825d-4cb5-9c92-14b0b9bd17fa", null, "Cordinator", "CORDINATOR" }
                });
        }
    }
}

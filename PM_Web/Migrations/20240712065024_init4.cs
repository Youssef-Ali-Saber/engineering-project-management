using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d9348fa-cc5b-4352-a3b6-d956f6b79765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a429a0c7-a7fc-4cfe-8149-81df29bdf080");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "763c1c62-d7aa-4dd3-a19e-fa122d15c6a5", "b85a0f58-5f82-474b-9a7a-e8215e3f9844", "TeamMember", "TEAMMEMBER" },
                    { "b19a4fde-3230-45b6-b313-665cb7ad5c3a", "e51c4520-99c3-49f8-8e22-08a01dc3ee3f", "Cordinator", "CORDINATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "763c1c62-d7aa-4dd3-a19e-fa122d15c6a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b19a4fde-3230-45b6-b313-665cb7ad5c3a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d9348fa-cc5b-4352-a3b6-d956f6b79765", "abad08bc-64e5-4987-a0b3-17c47575e1da", "Cordinator", "CORDINATOR" },
                    { "a429a0c7-a7fc-4cfe-8149-81df29bdf080", "21f85523-ab03-4b10-bb9a-c4ce33ac13eb", "TeamMember", "TEAMMEMBER" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class init0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41843928-eebe-4a3c-81d2-9d003465de3e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9075630a-d711-4dc4-9b62-d283d143953f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6e24c303-014b-4943-8e31-df5c6ee98594", null, "Cordinator", "CORDINATOR" },
                    { "fa59ed3f-1221-4474-b916-583a6637dcae", null, "TeamMember", "TEAMMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e24c303-014b-4943-8e31-df5c6ee98594");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa59ed3f-1221-4474-b916-583a6637dcae");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "41843928-eebe-4a3c-81d2-9d003465de3e", null, "Cordinator", "CORDINATOR" },
                    { "9075630a-d711-4dc4-9b62-d283d143953f", null, "TeamMember", "TEAMMEMBER" }
                });
        }
    }
}

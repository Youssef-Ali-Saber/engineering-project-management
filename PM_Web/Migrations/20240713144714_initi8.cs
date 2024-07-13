using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class initi8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "277a9e7e-5799-4d18-8d21-7195b9723024");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fd18b3e-ffcd-42e7-a31d-897f1d1afe39");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1e6b60b-4201-4007-bc54-906c1aa9e5d4");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Organization",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4523f160-034d-4df2-924d-095ec0493ee2", "03f74990-6438-4f1f-a616-faf21ebff9bd", "TeamManager", "TEAMMANAGER" },
                    { "7e747a20-cb51-4ea1-87ea-19ab6f0bb8bd", "2ca2499c-4a76-4efd-b47e-a51a87fc1188", "Cordinator", "CORDINATOR" },
                    { "bc1ca5e0-a673-4860-a898-667bf27dcdc6", "2b7e493a-9b84-4567-8bba-d6c7a99e7c10", "TeamMember", "TEAMMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4523f160-034d-4df2-924d-095ec0493ee2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e747a20-cb51-4ea1-87ea-19ab6f0bb8bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc1ca5e0-a673-4860-a898-667bf27dcdc6");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Organization",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "277a9e7e-5799-4d18-8d21-7195b9723024", "04ec1a0a-e678-4c82-a577-962cd798cd14", "TeamManager", "TEAMMANAGER" },
                    { "3fd18b3e-ffcd-42e7-a31d-897f1d1afe39", "4eda0f8f-8f47-4b2a-86a6-07b27ea90ac4", "TeamMember", "TEAMMEMBER" },
                    { "b1e6b60b-4201-4007-bc54-906c1aa9e5d4", "3d2f7476-66ce-43a9-b77e-9203ed075e58", "Cordinator", "CORDINATOR" }
                });
        }
    }
}

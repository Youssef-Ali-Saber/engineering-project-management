using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class initi7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_TeamManagerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TeamManagerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f53a9227-dcd4-4e04-af8f-0664311f81c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f88055b1-3251-490a-81cd-ef08baa10d6e");

            migrationBuilder.DropColumn(
                name: "TeamManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "TeamManager",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamMembers",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "TeamManager",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TeamMembers",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "TeamManagerId",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f53a9227-dcd4-4e04-af8f-0664311f81c7", "3a9af1a6-2bfd-4d28-b183-e7af9949c151", "Cordinator", "CORDINATOR" },
                    { "f88055b1-3251-490a-81cd-ef08baa10d6e", "9e5e653b-983c-4ac4-9d6a-b1f04d29aee7", "TeamMember", "TEAMMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TeamManagerId",
                table: "Projects",
                column: "TeamManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_TeamManagerId",
                table: "Projects",
                column: "TeamManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

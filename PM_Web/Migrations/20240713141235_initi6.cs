using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class initi6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "775899cf-c090-4e8f-af2a-e4373cb4e652");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da70b56d-2aec-4d04-9c73-6988cdc6c413");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "775899cf-c090-4e8f-af2a-e4373cb4e652", "09527e9f-f7b7-4897-9af7-ef869e85af30", "TeamMember", "TEAMMEMBER" },
                    { "da70b56d-2aec-4d04-9c73-6988cdc6c413", "4f2b08e1-98dd-489c-bec0-6a247ceb92de", "Cordinator", "CORDINATOR" }
                });
        }
    }
}

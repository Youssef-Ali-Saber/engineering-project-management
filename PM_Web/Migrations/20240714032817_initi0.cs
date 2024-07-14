using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class initi0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7edf9e8c-f8c0-47c1-9d65-d241241bd7b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b91975a0-b36d-4142-a205-79122aed01f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7b86157-ccaf-4f04-9464-2ef6363ceda5");

            migrationBuilder.CreateTable(
                name: "_System",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__System", x => x.Id);
                    table.ForeignKey(
                        name: "FK__System_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "915af4c7-05d8-4144-b52c-2a9222cbf6c6", "566d0f9b-74af-4f06-b6bc-d6c0684878b4", "Cordinator", "CORDINATOR" },
                    { "9d1f1242-546c-4940-9657-a192ac70520d", "82350318-5f5a-4ec0-8d8f-2da2c596ed06", "TeamManager", "TEAMMANAGER" },
                    { "9eb7d643-b75b-478d-8601-acf4e692e69b", "08ed1ef6-dbdc-461e-b252-8f7375e2c7a4", "TeamMember", "TEAMMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX__System_ProjectId",
                table: "_System",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_System");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "915af4c7-05d8-4144-b52c-2a9222cbf6c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d1f1242-546c-4940-9657-a192ac70520d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eb7d643-b75b-478d-8601-acf4e692e69b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7edf9e8c-f8c0-47c1-9d65-d241241bd7b5", "fa20c217-fd80-4f3f-a5a2-6be15c910ce3", "TeamManager", "TEAMMANAGER" },
                    { "b91975a0-b36d-4142-a205-79122aed01f3", "6bc966f2-9b45-4d4c-941d-837b549b95c5", "Cordinator", "CORDINATOR" },
                    { "f7b86157-ccaf-4f04-9464-2ef6363ceda5", "6fcb38b3-6ccf-4933-989d-702d6aaa554a", "TeamMember", "TEAMMEMBER" }
                });
        }
    }
}

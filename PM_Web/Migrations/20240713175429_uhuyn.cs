using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class uhuyn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34fc3f0d-85b8-497b-bc69-7406416f7fd8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71ab75ef-163b-483c-a593-42dfab140d66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7dd8653-e153-4a94-a0da-2f4c9a64fc37");

            migrationBuilder.DropColumn(
                name: "DocumentationDescriptions",
                table: "InterfacePoints");

            migrationBuilder.DropColumn(
                name: "DocumentationLinks",
                table: "InterfacePoints");

            migrationBuilder.CreateTable(
                name: "Documentation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentationLink = table.Column<string>(type: "TEXT", nullable: true),
                    DocumentationDescription = table.Column<string>(type: "TEXT", nullable: true),
                    InterfacePointId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentation_InterfacePoints_InterfacePointId",
                        column: x => x.InterfacePointId,
                        principalTable: "InterfacePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3415a636-2c20-4e8b-b684-983e53330f81", "c50ee15c-6146-467b-9ab2-eebe3ecccc72", "Cordinator", "CORDINATOR" },
                    { "3cba358d-7be4-48dc-9c1a-ade43e069450", "14ea58d4-aaa6-4374-b2da-718a09bdcf19", "TeamManager", "TEAMMANAGER" },
                    { "ed890c0a-6347-457d-9d1a-95d363ffd463", "250f1283-4624-450c-a55f-55f902d096fc", "TeamMember", "TEAMMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentation_InterfacePointId",
                table: "Documentation",
                column: "InterfacePointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documentation");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3415a636-2c20-4e8b-b684-983e53330f81");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cba358d-7be4-48dc-9c1a-ade43e069450");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed890c0a-6347-457d-9d1a-95d363ffd463");

            migrationBuilder.AddColumn<string>(
                name: "DocumentationDescriptions",
                table: "InterfacePoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentationLinks",
                table: "InterfacePoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34fc3f0d-85b8-497b-bc69-7406416f7fd8", "49f5cd98-5713-413c-80d1-d76cc8997b2f", "TeamManager", "TEAMMANAGER" },
                    { "71ab75ef-163b-483c-a593-42dfab140d66", "af17c579-aad4-4491-8a92-816c57403f74", "Cordinator", "CORDINATOR" },
                    { "c7dd8653-e153-4a94-a0da-2f4c9a64fc37", "61541b67-8c25-4eb4-976d-f3a16cf0e9d3", "TeamMember", "TEAMMEMBER" }
                });
        }
    }
}

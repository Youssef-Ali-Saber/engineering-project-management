using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class initi5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "763c1c62-d7aa-4dd3-a19e-fa122d15c6a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b19a4fde-3230-45b6-b313-665cb7ad5c3a");

            migrationBuilder.DropColumn(
                name: "Owners",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ScopePackages",
                table: "Projects",
                newName: "OwnerId");

            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Owner_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScopePackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopePackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopePackage_Projects_ProjectId",
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
                    { "775899cf-c090-4e8f-af2a-e4373cb4e652", "09527e9f-f7b7-4897-9af7-ef869e85af30", "TeamMember", "TEAMMEMBER" },
                    { "da70b56d-2aec-4d04-9c73-6988cdc6c413", "4f2b08e1-98dd-489c-bec0-6a247ceb92de", "Cordinator", "CORDINATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Owner_ProjectId",
                table: "Owner",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopePackage_ProjectId",
                table: "ScopePackage",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_OwnerId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Owner");

            migrationBuilder.DropTable(
                name: "ScopePackage");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "775899cf-c090-4e8f-af2a-e4373cb4e652");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da70b56d-2aec-4d04-9c73-6988cdc6c413");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Projects",
                newName: "ScopePackages");

            migrationBuilder.AddColumn<string>(
                name: "Owners",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "763c1c62-d7aa-4dd3-a19e-fa122d15c6a5", "b85a0f58-5f82-474b-9a7a-e8215e3f9844", "TeamMember", "TEAMMEMBER" },
                    { "b19a4fde-3230-45b6-b313-665cb7ad5c3a", "e51c4520-99c3-49f8-8e22-08a01dc3ee3f", "Cordinator", "CORDINATOR" }
                });
        }
    }
}

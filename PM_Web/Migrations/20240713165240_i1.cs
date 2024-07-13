using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class i1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectID",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_BOQs_Projects_ProjectID",
                table: "BOQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BOQs",
                table: "BOQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

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

            migrationBuilder.RenameTable(
                name: "BOQs",
                newName: "BOQ");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_BOQs_ProjectID",
                table: "BOQ",
                newName: "IX_BOQ_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ProjectID",
                table: "Activity",
                newName: "IX_Activity_ProjectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BOQ",
                table: "BOQ",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InterfacePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nature = table.Column<string>(type: "TEXT", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", nullable: false),
                    ScopePackage1 = table.Column<string>(type: "TEXT", nullable: false),
                    ScopePackage2 = table.Column<string>(type: "TEXT", nullable: true),
                    System1 = table.Column<string>(type: "TEXT", nullable: false),
                    System2 = table.Column<string>(type: "TEXT", nullable: true),
                    ExtraSystem = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    BOQId = table.Column<int>(type: "INTEGER", nullable: true),
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: true),
                    Responsible = table.Column<string>(type: "TEXT", nullable: true),
                    Consultant = table.Column<string>(type: "TEXT", nullable: true),
                    Accountable = table.Column<string>(type: "TEXT", nullable: true),
                    Informed = table.Column<string>(type: "TEXT", nullable: true),
                    Supported = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    CreatDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DocumentationLinks = table.Column<string>(type: "TEXT", nullable: true),
                    DocumentationDescriptions = table.Column<string>(type: "TEXT", nullable: true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterfacePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterfacePoints_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterfacePoints_BOQ_BOQId",
                        column: x => x.BOQId,
                        principalTable: "BOQ",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterfacePoints_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Sender = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InterfacePointId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_InterfacePoints_InterfacePointId",
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
                    { "34fc3f0d-85b8-497b-bc69-7406416f7fd8", "49f5cd98-5713-413c-80d1-d76cc8997b2f", "TeamManager", "TEAMMANAGER" },
                    { "71ab75ef-163b-483c-a593-42dfab140d66", "af17c579-aad4-4491-8a92-816c57403f74", "Cordinator", "CORDINATOR" },
                    { "c7dd8653-e153-4a94-a0da-2f4c9a64fc37", "61541b67-8c25-4eb4-976d-f3a16cf0e9d3", "TeamMember", "TEAMMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chat_InterfacePointId",
                table: "Chat",
                column: "InterfacePointId");

            migrationBuilder.CreateIndex(
                name: "IX_InterfacePoints_ActivityId",
                table: "InterfacePoints",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_InterfacePoints_BOQId",
                table: "InterfacePoints",
                column: "BOQId");

            migrationBuilder.CreateIndex(
                name: "IX_InterfacePoints_ProjectId",
                table: "InterfacePoints",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Projects_ProjectID",
                table: "Activity",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BOQ_Projects_ProjectID",
                table: "BOQ",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Projects_ProjectID",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_BOQ_Projects_ProjectID",
                table: "BOQ");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "InterfacePoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BOQ",
                table: "BOQ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

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

            migrationBuilder.RenameTable(
                name: "BOQ",
                newName: "BOQs");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_BOQ_ProjectID",
                table: "BOQs",
                newName: "IX_BOQs_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ProjectID",
                table: "Activities",
                newName: "IX_Activities_ProjectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BOQs",
                table: "BOQs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4523f160-034d-4df2-924d-095ec0493ee2", "03f74990-6438-4f1f-a616-faf21ebff9bd", "TeamManager", "TEAMMANAGER" },
                    { "7e747a20-cb51-4ea1-87ea-19ab6f0bb8bd", "2ca2499c-4a76-4efd-b47e-a51a87fc1188", "Cordinator", "CORDINATOR" },
                    { "bc1ca5e0-a673-4860-a898-667bf27dcdc6", "2b7e493a-9b84-4567-8bba-d6c7a99e7c10", "TeamMember", "TEAMMEMBER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectID",
                table: "Activities",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BOQs_Projects_ProjectID",
                table: "BOQs",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PM01.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e24c303-014b-4943-8e31-df5c6ee98594");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa59ed3f-1221-4474-b916-583a6637dcae");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectName = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectNature = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectType = table.Column<string>(type: "TEXT", nullable: false),
                    ScopePackages = table.Column<string>(type: "TEXT", nullable: false),
                    JVPartners = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    ProjectStage = table.Column<string>(type: "TEXT", nullable: false),
                    DeliveryStrategies = table.Column<string>(type: "TEXT", nullable: false),
                    ContractingStrategies = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BOQs_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e3863a0-1ed1-4440-a57b-2f6699a6d594", null, "TeamMember", "TEAMMEMBER" },
                    { "f596a376-825d-4cb5-9c92-14b0b9bd17fa", null, "Cordinator", "CORDINATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProjectID",
                table: "Activities",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_BOQs_ProjectID",
                table: "BOQs",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "BOQs");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e3863a0-1ed1-4440-a57b-2f6699a6d594");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f596a376-825d-4cb5-9c92-14b0b9bd17fa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6e24c303-014b-4943-8e31-df5c6ee98594", null, "Cordinator", "CORDINATOR" },
                    { "fa59ed3f-1221-4474-b916-583a6637dcae", null, "TeamMember", "TEAMMEMBER" }
                });
        }
    }
}

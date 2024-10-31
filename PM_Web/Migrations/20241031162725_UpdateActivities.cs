using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_InterfacePoints_InterfacePointId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_InterfacePointId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "InterfacePointId",
                table: "Activities");

            migrationBuilder.CreateTable(
                name: "ActivityInterfacePoint",
                columns: table => new
                {
                    ActivitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    InterfacePointsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityInterfacePoint", x => new { x.ActivitiesId, x.InterfacePointsId });
                    table.ForeignKey(
                        name: "FK_ActivityInterfacePoint_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityInterfacePoint_InterfacePoints_InterfacePointsId",
                        column: x => x.InterfacePointsId,
                        principalTable: "InterfacePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInterfacePoint_InterfacePointsId",
                table: "ActivityInterfacePoint",
                column: "InterfacePointsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityInterfacePoint");

            migrationBuilder.AddColumn<int>(
                name: "InterfacePointId",
                table: "Activities",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_InterfacePointId",
                table: "Activities",
                column: "InterfacePointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_InterfacePoints_InterfacePointId",
                table: "Activities",
                column: "InterfacePointId",
                principalTable: "InterfacePoints",
                principalColumn: "Id");
        }
    }
}

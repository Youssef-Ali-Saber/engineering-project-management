using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBOQs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BOQs_InterfacePoints_InterfacePointId",
                table: "BOQs");

            migrationBuilder.DropIndex(
                name: "IX_BOQs_InterfacePointId",
                table: "BOQs");

            migrationBuilder.DropColumn(
                name: "InterfacePointId",
                table: "BOQs");

            migrationBuilder.CreateTable(
                name: "BOQInterfacePoint",
                columns: table => new
                {
                    BOQsId = table.Column<int>(type: "INTEGER", nullable: false),
                    InterfacePointsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOQInterfacePoint", x => new { x.BOQsId, x.InterfacePointsId });
                    table.ForeignKey(
                        name: "FK_BOQInterfacePoint_BOQs_BOQsId",
                        column: x => x.BOQsId,
                        principalTable: "BOQs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BOQInterfacePoint_InterfacePoints_InterfacePointsId",
                        column: x => x.InterfacePointsId,
                        principalTable: "InterfacePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOQInterfacePoint_InterfacePointsId",
                table: "BOQInterfacePoint",
                column: "InterfacePointsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOQInterfacePoint");

            migrationBuilder.AddColumn<int>(
                name: "InterfacePointId",
                table: "BOQs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BOQs_InterfacePointId",
                table: "BOQs",
                column: "InterfacePointId");

            migrationBuilder.AddForeignKey(
                name: "FK_BOQs_InterfacePoints_InterfacePointId",
                table: "BOQs",
                column: "InterfacePointId",
                principalTable: "InterfacePoints",
                principalColumn: "Id");
        }
    }
}

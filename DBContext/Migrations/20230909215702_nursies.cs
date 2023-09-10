using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class nursies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NurseID",
                table: "NurseBookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Experiance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Nurses_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NurseBookings_NurseID",
                table: "NurseBookings",
                column: "NurseID");

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_ApplicationUserID",
                table: "Nurses",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_NurseBookings_Nurses_NurseID",
                table: "NurseBookings",
                column: "NurseID",
                principalTable: "Nurses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NurseBookings_Nurses_NurseID",
                table: "NurseBookings");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropIndex(
                name: "IX_NurseBookings_NurseID",
                table: "NurseBookings");

            migrationBuilder.DropColumn(
                name: "NurseID",
                table: "NurseBookings");
        }
    }
}

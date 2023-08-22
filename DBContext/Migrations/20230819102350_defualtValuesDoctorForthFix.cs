using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class defualtValuesDoctorForthFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_PositionID",
                table: "Doctors",
                column: "PositionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorPositions_PositionID",
                table: "Doctors",
                column: "PositionID",
                principalTable: "DoctorPositions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorPositions_PositionID",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_PositionID",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PositionID",
                table: "Doctors");
        }
    }
}

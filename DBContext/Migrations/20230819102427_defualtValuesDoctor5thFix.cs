using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class defualtValuesDoctor5thFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorPositions_PositionID",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "PositionID",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorPositions_PositionID",
                table: "Doctors",
                column: "PositionID",
                principalTable: "DoctorPositions",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorPositions_PositionID",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "PositionID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorPositions_PositionID",
                table: "Doctors",
                column: "PositionID",
                principalTable: "DoctorPositions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class paymentsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SetteledDate",
                table: "DoctorBookingPayments",
                newName: "SettledDate");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "DoctorBookingPayments",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<double>(
                name: "DoctorProfit",
                table: "DoctorBookingPayments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ProfitPercentage",
                table: "DoctorBookingPayments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SystemProfit",
                table: "DoctorBookingPayments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorProfit",
                table: "DoctorBookingPayments");

            migrationBuilder.DropColumn(
                name: "ProfitPercentage",
                table: "DoctorBookingPayments");

            migrationBuilder.DropColumn(
                name: "SystemProfit",
                table: "DoctorBookingPayments");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "DoctorBookingPayments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "SettledDate",
                table: "DoctorBookingPayments",
                newName: "SetteledDate");
        }
    }
}

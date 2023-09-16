using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class createDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getDate()");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Complaints",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Complaints");
        }
    }
}

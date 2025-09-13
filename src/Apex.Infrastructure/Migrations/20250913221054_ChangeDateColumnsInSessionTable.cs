using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateColumnsInSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEnd",
                schema: "domain",
                table: "sessions");

            migrationBuilder.DropColumn(
                name: "DateStart",
                schema: "domain",
                table: "sessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "domain",
                table: "sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "domain",
                table: "sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "domain",
                table: "sessions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "domain",
                table: "sessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                schema: "domain",
                table: "sessions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                schema: "domain",
                table: "sessions",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}

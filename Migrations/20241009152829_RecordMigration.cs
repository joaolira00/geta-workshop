using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedGETA.Migrations
{
    /// <inheritdoc />
    public partial class RecordMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Diagnostic",
                table: "Records",
                newName: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "HospitalName",
                table: "Records",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Records",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "HospitalName",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Records",
                newName: "Diagnostic");
        }
    }
}

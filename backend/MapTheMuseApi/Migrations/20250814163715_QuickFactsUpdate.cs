using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class QuickFactsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuickFacts",
                table: "Destinations");

            // Recreate as jsonb (nullable: choose what you prefer)
            migrationBuilder.AddColumn<Dictionary<string, string>>(
                name: "QuickFacts",
                table: "Destinations",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuickFacts",
                table: "Destinations");
        }
    }
}

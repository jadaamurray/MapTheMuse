using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameDescriptionToSummary_AddDescriptionAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Itineraries",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Itineraries",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Destinations",
                newName: "Summary");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Destinations",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Continent",
                table: "Destinations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Destinations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "CultureHighlights",
                table: "Destinations",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>()
            );

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Destinations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Destinations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Destinations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
            // Backfill empty slugs with a lower-cased, URL-safe version + "-Id" to ensure uniqueness
            migrationBuilder.Sql("""
      UPDATE "Destinations"
      SET "Slug" =
        CASE
          WHEN COALESCE(NULLIF("Slug", ''), '') = ''
          THEN lower(regexp_replace("Name", '[^a-z0-9]+', '-', 'g')) || '-' || "Id"
          ELSE lower("Slug")
        END
    """);

            // Fix any accidental duplicates that already existed (append "-Id" to dupes)
            migrationBuilder.Sql("""
      WITH d AS (
        SELECT "Id","Slug",
               ROW_NUMBER() OVER (PARTITION BY "Slug" ORDER BY "Id") AS rn
        FROM "Destinations"
        WHERE COALESCE(NULLIF("Slug", ''), '') <> ''
      )
      UPDATE "Destinations" t
      SET "Slug" = t."Slug" || '-' || t."Id"
      FROM d
      WHERE t."Id" = d."Id" AND d.rn > 1
    """);

            // Create the unique index
            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Slug",
                table: "Destinations",
                column: "Slug",
                unique: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Destinations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbUrl",
                table: "Destinations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Itineraries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Itineraries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.DropIndex(
    name: "IX_Destinations_Slug",
    table: "Destinations");


            migrationBuilder.DropColumn(name: "Description", table: "Destinations");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Destinations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Destinations",
                newName: "Description");

            migrationBuilder.DropColumn(
            name: "Continent",
            table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CultureHighlights",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ThumbUrl",
                table: "Destinations");

        }
    }
}

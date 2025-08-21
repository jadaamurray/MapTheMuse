using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class MediaSpine_Phase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "Media",
                newName: "ExternalId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Media",
                type: "character varying(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Media",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncedUtc",
                table: "Media",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "Media",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Media",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Media",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "DestinationMediaLinks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_Source_Type_ExternalId",
                table: "Media",
                columns: new[] { "Source", "Type", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DestinationMediaLinks_MediaId",
                table: "DestinationMediaLinks",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DestinationMediaLinks_Media_MediaId",
                table: "DestinationMediaLinks",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DestinationMediaLinks_Media_MediaId",
                table: "DestinationMediaLinks");

            migrationBuilder.DropIndex(
                name: "IX_Media_Source_Type_ExternalId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_DestinationMediaLinks_MediaId",
                table: "DestinationMediaLinks");

            migrationBuilder.DropColumn(
                name: "LastSyncedUtc",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "DestinationMediaLinks");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "Media",
                newName: "MediaType");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Media",
                type: "character varying(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Media",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}

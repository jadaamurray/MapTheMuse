using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class ItineraryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArtEngagement_AspNetUsers_UserId",
                table: "UserArtEngagement");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArtEngagement_Destinations_DestinationId",
                table: "UserArtEngagement");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArtEngagement_PhysicalArtworks_PhysicalArtId",
                table: "UserArtEngagement");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagement_AspNetUsers_UserId",
                table: "UserMediaEngagement");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagement_Destinations_DestinationId",
                table: "UserMediaEngagement");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagement_Media_MediaId",
                table: "UserMediaEngagement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMediaEngagement",
                table: "UserMediaEngagement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArtEngagement",
                table: "UserArtEngagement");

            migrationBuilder.RenameTable(
                name: "UserMediaEngagement",
                newName: "UserMediaEngagements");

            migrationBuilder.RenameTable(
                name: "UserArtEngagement",
                newName: "UserArtEngagements");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaEngagement_UserId",
                table: "UserMediaEngagements",
                newName: "IX_UserMediaEngagements_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaEngagement_MediaId",
                table: "UserMediaEngagements",
                newName: "IX_UserMediaEngagements_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaEngagement_DestinationId",
                table: "UserMediaEngagements",
                newName: "IX_UserMediaEngagements_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArtEngagement_UserId",
                table: "UserArtEngagements",
                newName: "IX_UserArtEngagements_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArtEngagement_PhysicalArtId",
                table: "UserArtEngagements",
                newName: "IX_UserArtEngagements_PhysicalArtId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArtEngagement_DestinationId",
                table: "UserArtEngagements",
                newName: "IX_UserArtEngagements_DestinationId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PhysicalArtworks",
                type: "character varying(600)",
                maxLength: 600,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(600)",
                oldMaxLength: 600);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMediaEngagements",
                table: "UserMediaEngagements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArtEngagements",
                table: "UserArtEngagements",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itineraries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItineraryId = table.Column<int>(type: "integer", nullable: false),
                    DestinationId = table.Column<int>(type: "integer", nullable: false),
                    PhysicalArtId = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryItems_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryItems_Itineraries_ItineraryId",
                        column: x => x.ItineraryId,
                        principalTable: "Itineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryItems_PhysicalArtworks_PhysicalArtId",
                        column: x => x.PhysicalArtId,
                        principalTable: "PhysicalArtworks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_UserId",
                table: "Itineraries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryItems_DestinationId",
                table: "ItineraryItems",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryItems_ItineraryId",
                table: "ItineraryItems",
                column: "ItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryItems_PhysicalArtId",
                table: "ItineraryItems",
                column: "PhysicalArtId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserArtEngagements_AspNetUsers_UserId",
                table: "UserArtEngagements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArtEngagements_Destinations_DestinationId",
                table: "UserArtEngagements",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArtEngagements_PhysicalArtworks_PhysicalArtId",
                table: "UserArtEngagements",
                column: "PhysicalArtId",
                principalTable: "PhysicalArtworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagements_AspNetUsers_UserId",
                table: "UserMediaEngagements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagements_Destinations_DestinationId",
                table: "UserMediaEngagements",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagements_Media_MediaId",
                table: "UserMediaEngagements",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArtEngagements_AspNetUsers_UserId",
                table: "UserArtEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArtEngagements_Destinations_DestinationId",
                table: "UserArtEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArtEngagements_PhysicalArtworks_PhysicalArtId",
                table: "UserArtEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagements_AspNetUsers_UserId",
                table: "UserMediaEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagements_Destinations_DestinationId",
                table: "UserMediaEngagements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagements_Media_MediaId",
                table: "UserMediaEngagements");

            migrationBuilder.DropTable(
                name: "ItineraryItems");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMediaEngagements",
                table: "UserMediaEngagements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArtEngagements",
                table: "UserArtEngagements");

            migrationBuilder.RenameTable(
                name: "UserMediaEngagements",
                newName: "UserMediaEngagement");

            migrationBuilder.RenameTable(
                name: "UserArtEngagements",
                newName: "UserArtEngagement");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaEngagements_UserId",
                table: "UserMediaEngagement",
                newName: "IX_UserMediaEngagement_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaEngagements_MediaId",
                table: "UserMediaEngagement",
                newName: "IX_UserMediaEngagement_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaEngagements_DestinationId",
                table: "UserMediaEngagement",
                newName: "IX_UserMediaEngagement_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArtEngagements_UserId",
                table: "UserArtEngagement",
                newName: "IX_UserArtEngagement_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArtEngagements_PhysicalArtId",
                table: "UserArtEngagement",
                newName: "IX_UserArtEngagement_PhysicalArtId");

            migrationBuilder.RenameIndex(
                name: "IX_UserArtEngagements_DestinationId",
                table: "UserArtEngagement",
                newName: "IX_UserArtEngagement_DestinationId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PhysicalArtworks",
                type: "character varying(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(600)",
                oldMaxLength: 600,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMediaEngagement",
                table: "UserMediaEngagement",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArtEngagement",
                table: "UserArtEngagement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserArtEngagement_AspNetUsers_UserId",
                table: "UserArtEngagement",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArtEngagement_Destinations_DestinationId",
                table: "UserArtEngagement",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArtEngagement_PhysicalArtworks_PhysicalArtId",
                table: "UserArtEngagement",
                column: "PhysicalArtId",
                principalTable: "PhysicalArtworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagement_AspNetUsers_UserId",
                table: "UserMediaEngagement",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagement_Destinations_DestinationId",
                table: "UserMediaEngagement",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagement_Media_MediaId",
                table: "UserMediaEngagement",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

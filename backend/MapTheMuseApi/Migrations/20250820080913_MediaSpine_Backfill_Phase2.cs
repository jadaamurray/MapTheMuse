using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    public partial class MediaSpine_Backfill_Phase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Insert missing Media rows (distinct triples) from DML
            // Normalise MediaType values into enum string names (Movie, Tv, Book, Music, Artwork, Album, Song).
            migrationBuilder.Sql(@"
INSERT INTO ""Media"" (""Source"", ""Type"", ""ExternalId"", ""LastSyncedUtc"")
SELECT DISTINCT
    dml.""Source"",
    CASE LOWER(dml.""MediaType"")
        WHEN 'movie'   THEN 'Movie'
        WHEN 'tv'      THEN 'Tv'
        WHEN 'book'    THEN 'Book'
        WHEN 'music'   THEN 'Music'
        WHEN 'album'   THEN 'Album'
        WHEN 'song'    THEN 'Song'
        WHEN 'artwork' THEN 'Artwork'
        ELSE 'Movie'
    END AS ""Type"",
    dml.""ExternalId"",
    NOW()
FROM ""DestinationMediaLinks"" dml
LEFT JOIN ""Media"" m
    ON m.""Source"" = dml.""Source""
   AND m.""ExternalId"" = dml.""ExternalId""
   AND m.""Type"" = CASE LOWER(dml.""MediaType"")
                        WHEN 'movie'   THEN 'Movie'
                        WHEN 'tv'      THEN 'Tv'
                        WHEN 'book'    THEN 'Book'
                        WHEN 'music'   THEN 'Music'
                        WHEN 'album'   THEN 'Album'
                        WHEN 'song'    THEN 'Song'
                        WHEN 'artwork' THEN 'Artwork'
                        ELSE 'Movie'
                    END
WHERE m.""Id"" IS NULL;
");

            // 2) Backfill MediaId in DML by joining to Media on the triple
            migrationBuilder.Sql(@"
UPDATE ""DestinationMediaLinks"" dml
SET ""MediaId"" = m.""Id""
FROM ""Media"" m
WHERE m.""Source"" = dml.""Source""
  AND m.""ExternalId"" = dml.""ExternalId""
  AND m.""Type"" = CASE LOWER(dml.""MediaType"")
                      WHEN 'movie'   THEN 'Movie'
                      WHEN 'tv'      THEN 'Tv'
                      WHEN 'book'    THEN 'Book'
                      WHEN 'music'   THEN 'Music'
                      WHEN 'album'   THEN 'Album'
                      WHEN 'song'    THEN 'Song'
                      WHEN 'artwork' THEN 'Artwork'
                      ELSE 'Movie'
                   END;
");

            // Optional sanity check (will throw if any row failed to backfill)
            // If you prefer silent behaviour, remove this block.
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF EXISTS (SELECT 1 FROM ""DestinationMediaLinks"" WHERE ""MediaId"" IS NULL) THEN
    RAISE EXCEPTION 'Backfill failed: some DestinationMediaLinks.MediaId remain NULL';
  END IF;
END$$;
");

            // 3) Make MediaId required + add/ensure FK and a helpful de-dup index on (DestinationId, MediaId)
            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "DestinationMediaLinks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DestinationMediaLinks_DestinationId_MediaId",
                table: "DestinationMediaLinks",
                columns: new[] { "DestinationId", "MediaId" });

            // 4) Drop the old triple columns from DML
            migrationBuilder.DropColumn(
                name: "Source",
                table: "DestinationMediaLinks");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "DestinationMediaLinks");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "DestinationMediaLinks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate old columns on DML
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "DestinationMediaLinks",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "TMDB");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "DestinationMediaLinks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "DestinationMediaLinks",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                defaultValue: "Movie");

            // Rehydrate old columns from Media via MediaId
            migrationBuilder.Sql(@"
UPDATE ""DestinationMediaLinks"" dml
SET ""Source""    = m.""Source"",
    ""ExternalId""= m.""ExternalId"",
    ""MediaType"" = m.""Type""
FROM ""Media"" m
WHERE dml.""MediaId"" = m.""Id"";
");

            // Drop helper index
            migrationBuilder.DropIndex(
                name: "IX_DestinationMediaLinks_DestinationId_MediaId",
                table: "DestinationMediaLinks");

            // Make MediaId nullable again (and optionally drop FK)
            migrationBuilder.DropForeignKey(
                name: "FK_DestinationMediaLinks_Media_MediaId",
                table: "DestinationMediaLinks");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "DestinationMediaLinks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            // (We keep the inserted Media rows; no need to delete.)
        }
    }
}

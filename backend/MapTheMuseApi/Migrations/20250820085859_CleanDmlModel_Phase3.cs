using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class CleanDmlModel_Phase3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE ""DestinationMediaLinks"" DROP COLUMN IF EXISTS ""ExternalId"";
ALTER TABLE ""DestinationMediaLinks"" DROP COLUMN IF EXISTS ""Source"";
ALTER TABLE ""DestinationMediaLinks"" DROP COLUMN IF EXISTS ""MediaType"";
");

// Unique index on Media identity
migrationBuilder.Sql(@"
DO $$
BEGIN
  IF NOT EXISTS (
    SELECT 1 FROM pg_indexes 
    WHERE schemaname = 'public' 
      AND indexname = 'IX_Media_Source_Type_ExternalId'
  ) THEN
    CREATE UNIQUE INDEX ""IX_Media_Source_Type_ExternalId""
      ON ""Media"" (""Source"", ""Type"", ""ExternalId"");
  END IF;
END $$;
");

// FK on DestinationMediaLinks(MediaId) -> Media(Id)
migrationBuilder.Sql(@"
DO $$
BEGIN
  IF NOT EXISTS (
    SELECT 1 FROM pg_constraint
    WHERE conname = 'FK_DestinationMediaLinks_Media_MediaId'
  ) THEN
    ALTER TABLE ""DestinationMediaLinks""
    ADD CONSTRAINT ""FK_DestinationMediaLinks_Media_MediaId""
    FOREIGN KEY (""MediaId"") REFERENCES ""Media""(""Id"") ON DELETE CASCADE;
  END IF;
END $$;
");

// Helpful de-dup index per destination
migrationBuilder.Sql(@"
DO $$
BEGIN
  IF NOT EXISTS (
    SELECT 1 FROM pg_indexes 
    WHERE schemaname = 'public' 
      AND indexname = 'IX_DestinationMediaLinks_DestinationId_MediaId'
  ) THEN
    CREATE INDEX ""IX_DestinationMediaLinks_DestinationId_MediaId""
      ON ""DestinationMediaLinks"" (""DestinationId"", ""MediaId"");
  END IF;
END $$;
");

// Only make NOT NULL if column exists
migrationBuilder.Sql(@"
DO $$
BEGIN
  IF EXISTS (
    SELECT 1 FROM information_schema.columns
    WHERE table_schema = 'public'
      AND table_name = 'DestinationMediaLinks'
      AND column_name = 'MediaId'
  ) THEN
    ALTER TABLE ""DestinationMediaLinks"" ALTER COLUMN ""MediaId"" SET NOT NULL;
  END IF;
END $$;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "DestinationMediaLinks",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }
    }
}

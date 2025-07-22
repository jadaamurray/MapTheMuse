using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscriminatorToAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "AppUser");
            migrationBuilder.Sql(
                "UPDATE \"AspNetUsers\" SET \"Discriminator\" = 'AppUser' WHERE \"Discriminator\" IS NULL OR \"Discriminator\" = ''");

        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
           name: "Discriminator",
           table: "AspNetUsers");
        }
    }
}
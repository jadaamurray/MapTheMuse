using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapTheMuseApi.Migrations
{
    /// <inheritdoc />
    public partial class housekeeping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagements_Destinations_DestinationId",
                table: "UserMediaEngagements");

            migrationBuilder.AlterColumn<int>(
                name: "DestinationId",
                table: "UserMediaEngagements",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagements_Destinations_DestinationId",
                table: "UserMediaEngagements",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaEngagements_Destinations_DestinationId",
                table: "UserMediaEngagements");

            migrationBuilder.AlterColumn<int>(
                name: "DestinationId",
                table: "UserMediaEngagements",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaEngagements_Destinations_DestinationId",
                table: "UserMediaEngagements",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

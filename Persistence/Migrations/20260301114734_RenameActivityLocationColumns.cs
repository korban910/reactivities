using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameActivityLocationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location_Venue",
                table: "Activities",
                newName: "Venue");

            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Activities",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Activities",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "Location_City",
                table: "Activities",
                newName: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Venue",
                table: "Activities",
                newName: "Location_Venue");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Activities",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Activities",
                newName: "Location_Latitude");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Activities",
                newName: "Location_City");
        }
    }
}

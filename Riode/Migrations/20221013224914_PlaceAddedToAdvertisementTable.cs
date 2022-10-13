using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Riode.Migrations
{
    public partial class PlaceAddedToAdvertisementTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Place",
                table: "Advertisements",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Advertisements");
        }
    }
}

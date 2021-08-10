using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Core.Migrations
{
    public partial class ZipCodeRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Countries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Countries",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);
        }
    }
}

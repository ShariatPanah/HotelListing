using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Data.Migrations
{
    public partial class Testing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e79c672-ae67-4012-8b04-9de649c17ea6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a29683ff-edf2-4bfa-ac5c-33314f4d925b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba9a9b2f-a412-4e28-81cc-4d0a5c3341d4", "8f632a2b-cdb3-4cfa-b59c-9e6c98929147", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a0fbb4f1-5c50-4fd6-91a7-9b6c7804c156", "51d9d1bb-51a5-4b7c-bad9-690d032746fe", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0fbb4f1-5c50-4fd6-91a7-9b6c7804c156");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba9a9b2f-a412-4e28-81cc-4d0a5c3341d4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e79c672-ae67-4012-8b04-9de649c17ea6", "e2308c9f-b368-470a-b1cf-bde16a88e1b3", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a29683ff-edf2-4bfa-ac5c-33314f4d925b", "90c75ea3-e8a8-4980-ba97-35203ad1497e", "Admin", "ADMIN" });
        }
    }
}

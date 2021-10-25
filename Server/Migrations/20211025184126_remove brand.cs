using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorShoppingApp.Server.Migrations
{
    public partial class removebrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

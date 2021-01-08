using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingShop.WebApp.Migrations.AppDb
{
    public partial class OrderStateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "UserOrders",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "UserOrders");
        }
    }
}

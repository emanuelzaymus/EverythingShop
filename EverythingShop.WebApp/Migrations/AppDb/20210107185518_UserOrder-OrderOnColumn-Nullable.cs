using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingShop.WebApp.Migrations.AppDb
{
    public partial class UserOrderOrderOnColumnNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "OrderedOn",
              table: "UserOrders",
              nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "OrderedOn",
              table: "UserOrders",
              nullable: false);
        }
    }
}

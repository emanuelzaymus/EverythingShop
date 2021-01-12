using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingShop.WebApp.Migrations.AppDb
{
    public partial class DropColumnPictureFromCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "MainCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "MainCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

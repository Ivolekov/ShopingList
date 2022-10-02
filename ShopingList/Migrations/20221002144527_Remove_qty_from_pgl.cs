using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopingList.Migrations
{
    public partial class Remove_qty_from_pgl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Product_GroceryLists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Product_GroceryLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

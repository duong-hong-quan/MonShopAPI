using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonShop.Library.Migrations
{
    public partial class AddTableSizeAndProductInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeId);
                });

            migrationBuilder.CreateTable(
                name: "ProductInventory",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventory", x => new { x.ProductId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_ProductInventory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInventory_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "SizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_SizeId",
                table: "OrderItem",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_SizeId",
                table: "CartItem",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventory_SizeId",
                table: "ProductInventory",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Size_SizeId",
                table: "CartItem",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "SizeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Size_SizeId",
                table: "OrderItem",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "SizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Size_SizeId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Size_SizeId",
                table: "OrderItem");

            migrationBuilder.DropTable(
                name: "ProductInventory");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_SizeId",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_SizeId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "CartItem");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

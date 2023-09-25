using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonShop.Library.Migrations
{
    public partial class UpdateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "CategoryId", "Description", "Discount", "ImageUrl", "IsDeleted", "Price", "ProductName", "ProductStatusId" },
                values: new object[,]
                {
                    { 1, 1, "This is the description for Product 1.", 5.0, "image1.jpg", false, 19.989999999999998, "Product 1", 1 },
                    { 2, 2, "This is the description for Product 2.", null, "image2.jpg", false, 29.989999999999998, "Product 2", 1 },
                    { 3, 1, null, null, null, true, 9.9900000000000002, "Product 3", 2 }
                });

            migrationBuilder.InsertData(
                table: "Size",
                columns: new[] { "SizeId", "SizeName" },
                values: new object[,]
                {
                    { 1, "S" },
                    { 2, "M" },
                    { 3, "L" },
                    { 4, "XL" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeId",
                keyValue: 4);
        }
    }
}

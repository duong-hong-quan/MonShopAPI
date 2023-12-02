using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonShop.BackEnd.DAL.Migrations
{
    public partial class updatefieldcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "334fadb1-b04a-44b0-bef5-82ecc53d513b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47adbc5f-e6bd-40b4-b431-5d5b16eab51d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b8cfbc7-303e-4c7f-921d-736a2b60b6ee");

            migrationBuilder.AddColumn<string>(
                name: "CategoryDescription",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryImgUrl",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "874ea9b8-6be5-48e5-9faf-288bc9ba6997", "61fad947-1892-468e-9a3c-cf557f6edc57", "ADMIN", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9a840661-cf41-480b-9897-f5641ba3028a", "7ee6c14f-494e-4200-a942-8ad49e125d99", "STAFF", "staff" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e8d68a76-f121-4013-9526-11101935b27c", "9f92cee0-b910-47e2-b025-ddbc398bdeed", "USER", "user" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "874ea9b8-6be5-48e5-9faf-288bc9ba6997");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a840661-cf41-480b-9897-f5641ba3028a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8d68a76-f121-4013-9526-11101935b27c");

            migrationBuilder.DropColumn(
                name: "CategoryDescription",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryImgUrl",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Category");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "334fadb1-b04a-44b0-bef5-82ecc53d513b", "2b87a976-3891-4cd2-a9e7-ec3391be9a28", "ADMIN", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "47adbc5f-e6bd-40b4-b431-5d5b16eab51d", "d1c19e72-d145-4177-a98d-29d3a109de1c", "USER", "user" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6b8cfbc7-303e-4c7f-921d-736a2b60b6ee", "fb5fc4a9-c6fe-4bb5-ae08-2ff65201aeec", "STAFF", "staff" });
        }
    }
}

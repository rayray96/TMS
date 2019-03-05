using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27547609-6d11-4895-921f-eed008292079");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "607643b0-c4b3-4e0e-bdef-b0035ba3ea60");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6128b66e-e9c1-45b3-9770-b721c308db3f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9beb6a5b-147d-4a26-a9c8-20154e78a13c", "0df59d58-09cc-4620-952e-2c526945c7cd", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9d54dacc-21ac-43dd-aadc-ac72348cd935", "99d59588-9fd8-4d6c-ab56-b64e4fd7e745", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3c3cc97c-f2de-46d5-9ac8-042e9cbcf127", "915e5c93-7e33-4b13-9ef1-07b7ef7f9c10", "Worker", "WORKER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c3cc97c-f2de-46d5-9ac8-042e9cbcf127");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9beb6a5b-147d-4a26-a9c8-20154e78a13c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d54dacc-21ac-43dd-aadc-ac72348cd935");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "27547609-6d11-4895-921f-eed008292079", "2dc17fe2-5fe7-44ff-961b-6afd1fe41856", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6128b66e-e9c1-45b3-9770-b721c308db3f", "10640e6e-db0c-4e92-a142-e814b6976d66", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "607643b0-c4b3-4e0e-bdef-b0035ba3ea60", "b8080b80-6ba2-44cf-909d-a1e91e868000", "Worker", "WORKER" });
        }
    }
}

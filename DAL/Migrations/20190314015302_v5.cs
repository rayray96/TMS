using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ebb95e4-b38a-4e5a-93e9-43c61b159184");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73cb8b3f-ba77-4c50-8ddc-e72675f5964e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e681eb0d-c007-49ae-b634-921f2a055a88");

            migrationBuilder.AlterColumn<string>(
                name: "LName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2de5e722-43ac-4c5c-9c6b-8f13bd26ada7", "b4090688-e453-401b-9d62-97b611c238ce", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8e295fc9-6f4e-4a74-a0fb-c5345bf82494", "4280c9cb-f9c8-4f9e-889d-7cf6a71450a7", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "52ad0df6-58b5-4c58-bdfd-4fbc28e25c0a", "5ade07fd-42db-4e8c-a9cb-10b1e30660e2", "Worker", "WORKER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2de5e722-43ac-4c5c-9c6b-8f13bd26ada7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52ad0df6-58b5-4c58-bdfd-4fbc28e25c0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e295fc9-6f4e-4a74-a0fb-c5345bf82494");

            migrationBuilder.AlterColumn<string>(
                name: "LName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3ebb95e4-b38a-4e5a-93e9-43c61b159184", "569b6af2-bec0-409f-a74f-27aa6042498a", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e681eb0d-c007-49ae-b634-921f2a055a88", "426c8117-4a91-4d3a-9ffd-c40a99a3bf23", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "73cb8b3f-ba77-4c50-8ddc-e72675f5964e", "792de438-b4b8-49aa-86db-cfa62f013e2d", "Worker", "WORKER" });
        }
    }
}

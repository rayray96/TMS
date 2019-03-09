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
                keyValue: "3a44e138-0561-4273-8fea-f129ec62610c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5579f0d-8cbb-40b5-b00a-bdf4c08e880d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b589b466-d1de-4210-90bf-2c884a365200");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "25d6cdfc-83b2-40d9-b172-21993759ab61", "812a6f03-76c4-45ad-a54a-472c1e5539ac", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "440d099e-a6ee-46e6-96d2-3c6f6fe05c6c", "db69877d-0632-41a1-a55f-caf85ce78107", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "84d28b86-2e45-4b9e-b6ee-6f3be5c97c19", "f6833ac1-e1c5-4e67-b9ba-dd9c526533ce", "Worker", "WORKER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25d6cdfc-83b2-40d9-b172-21993759ab61");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "440d099e-a6ee-46e6-96d2-3c6f6fe05c6c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84d28b86-2e45-4b9e-b6ee-6f3be5c97c19");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b5579f0d-8cbb-40b5-b00a-bdf4c08e880d", "32333c9a-ef96-4bc5-957b-fb352f04f1d9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b589b466-d1de-4210-90bf-2c884a365200", "f8553781-72ef-414c-8f92-20ade22a9024", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3a44e138-0561-4273-8fea-f129ec62610c", "e11d7984-0d13-487f-938d-5804cbf5a252", "Worker", "WORKER" });
        }
    }
}

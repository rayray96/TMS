using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "427ebde8-6e7b-4f19-abc6-c6518e424253");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7654b68-c8e0-461e-a0c9-36a536c7e018");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0d38e07-a6b2-45ae-afa8-2bdd3da4b543");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "Refreshtoken",
                table: "RefreshTokens",
                newName: "Token");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Issue",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FName",
                table: "People",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LName",
                table: "People",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "People",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LName",
                table: "AspNetUsers",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Issue",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "FName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "LName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "FName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "Refreshtoken");

            migrationBuilder.AddColumn<bool>(
                name: "Revoked",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "People",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "427ebde8-6e7b-4f19-abc6-c6518e424253", "c62fe55c-1da4-40ca-9342-3e30427f396f", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c7654b68-c8e0-461e-a0c9-36a536c7e018", "347c1076-e13c-4714-a77e-47ddfb453bd2", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e0d38e07-a6b2-45ae-afa8-2bdd3da4b543", "aa3886e4-d58a-4978-ba18-ac299768aae9", "Worker", "WORKER" });
        }
    }
}

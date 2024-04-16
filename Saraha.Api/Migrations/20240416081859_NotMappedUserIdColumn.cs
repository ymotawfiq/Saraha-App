using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class NotMappedUserIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83e8c640-c0bb-4fd7-98ca-a5290947db54");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e47c6a7-2ca7-4167-891e-f781e904068b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "53278a29-5682-4b1d-b69e-1c125a0f2ca5", "1", "Admin", "Admin" },
                    { "dcb1d03d-b9b5-4807-a289-2833b0a4b124", "1", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "53278a29-5682-4b1d-b69e-1c125a0f2ca5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcb1d03d-b9b5-4807-a289-2833b0a4b124");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83e8c640-c0bb-4fd7-98ca-a5290947db54", "1", "User", "User" },
                    { "8e47c6a7-2ca7-4167-891e-f781e904068b", "1", "Admin", "Admin" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class NotMappedSendUserMessageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78e19bef-5fa4-4d8a-b3f1-3ddb6384844b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9edf1f04-ac3c-4b5f-a44e-b1acfc0addda");

            migrationBuilder.DropColumn(
                name: "SendUserEmail",
                table: "UserMessages");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83e8c640-c0bb-4fd7-98ca-a5290947db54", "1", "User", "User" },
                    { "8e47c6a7-2ca7-4167-891e-f781e904068b", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83e8c640-c0bb-4fd7-98ca-a5290947db54");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e47c6a7-2ca7-4167-891e-f781e904068b");

            migrationBuilder.AddColumn<string>(
                name: "SendUserEmail",
                table: "UserMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "78e19bef-5fa4-4d8a-b3f1-3ddb6384844b", "1", "Admin", "Admin" },
                    { "9edf1f04-ac3c-4b5f-a44e-b1acfc0addda", "1", "User", "User" }
                });
        }
    }
}

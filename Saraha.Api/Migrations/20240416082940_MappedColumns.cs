using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class MappedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "53278a29-5682-4b1d-b69e-1c125a0f2ca5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcb1d03d-b9b5-4807-a289-2833b0a4b124");

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
                    { "35a4a6e1-cd6b-4276-9f1a-bbe6ead5d391", "1", "User", "User" },
                    { "b0ab5bab-9923-4c31-baa4-ea23c4c511d5", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35a4a6e1-cd6b-4276-9f1a-bbe6ead5d391");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0ab5bab-9923-4c31-baa4-ea23c4c511d5");

            migrationBuilder.DropColumn(
                name: "SendUserEmail",
                table: "UserMessages");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "53278a29-5682-4b1d-b69e-1c125a0f2ca5", "1", "Admin", "Admin" },
                    { "dcb1d03d-b9b5-4807-a289-2833b0a4b124", "1", "User", "User" }
                });
        }
    }
}

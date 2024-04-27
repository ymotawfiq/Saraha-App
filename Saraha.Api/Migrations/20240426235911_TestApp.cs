using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class TestApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35a4a6e1-cd6b-4276-9f1a-bbe6ead5d391");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0ab5bab-9923-4c31-baa4-ea23c4c511d5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "35a4a6e1-cd6b-4276-9f1a-bbe6ead5d391", "1", "User", "User" },
                    { "b0ab5bab-9923-4c31-baa4-ea23c4c511d5", "1", "Admin", "Admin" }
                });
        }
    }
}

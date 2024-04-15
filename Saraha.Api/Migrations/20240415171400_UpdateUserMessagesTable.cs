using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserMessagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4cf22bcd-1c65-4933-b9b1-45b695c1287a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2932188-0819-4690-a8c6-d5a3962a0a47");

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
                    { "b642a5c7-1aae-411b-8869-07c5d97aaf79", "1", "Admin", "Admin" },
                    { "e971792f-3af9-4930-939b-4371d07cfdca", "1", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b642a5c7-1aae-411b-8869-07c5d97aaf79");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e971792f-3af9-4930-939b-4371d07cfdca");

            migrationBuilder.DropColumn(
                name: "SendUserEmail",
                table: "UserMessages");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4cf22bcd-1c65-4933-b9b1-45b695c1287a", "1", "Admin", "Admin" },
                    { "f2932188-0819-4690-a8c6-d5a3962a0a47", "1", "User", "User" }
                });
        }
    }
}

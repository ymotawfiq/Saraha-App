using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class AskToSendUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0af9ce0b-e9c6-4e58-b96e-931bdf3dd00b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a45fe40-8afe-4bd1-81a1-e63467b2a8a1");

            migrationBuilder.RenameColumn(
                name: "SendUserEmail",
                table: "UserMessages",
                newName: "SendUserName");

            migrationBuilder.AddColumn<bool>(
                name: "ShareYourUserName",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "844e173a-8ccb-447b-8947-77b6e07f60a9", "1", "Admin", "Admin" },
                    { "8506940e-c9cc-4997-a5f2-882f79bcd4f3", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "844e173a-8ccb-447b-8947-77b6e07f60a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8506940e-c9cc-4997-a5f2-882f79bcd4f3");

            migrationBuilder.DropColumn(
                name: "ShareYourUserName",
                table: "UserMessages");

            migrationBuilder.RenameColumn(
                name: "SendUserName",
                table: "UserMessages",
                newName: "SendUserEmail");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0af9ce0b-e9c6-4e58-b96e-931bdf3dd00b", "1", "Admin", "Admin" },
                    { "6a45fe40-8afe-4bd1-81a1-e63467b2a8a1", "2", "User", "User" }
                });
        }
    }
}

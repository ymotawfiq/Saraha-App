using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOnDeleteMessageBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b642a5c7-1aae-411b-8869-07c5d97aaf79");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e971792f-3af9-4930-939b-4371d07cfdca");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7409c9f4-4c5b-4517-a8ff-9f9271c2de88", "1", "Admin", "Admin" },
                    { "a7d9d34a-337d-4bc8-b3e1-eb57b5e38099", "1", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7409c9f4-4c5b-4517-a8ff-9f9271c2de88");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7d9d34a-337d-4bc8-b3e1-eb57b5e38099");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b642a5c7-1aae-411b-8869-07c5d97aaf79", "1", "Admin", "Admin" },
                    { "e971792f-3af9-4930-939b-4371d07cfdca", "1", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

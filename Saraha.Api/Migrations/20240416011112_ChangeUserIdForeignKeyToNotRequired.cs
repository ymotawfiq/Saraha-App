using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdForeignKeyToNotRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7409c9f4-4c5b-4517-a8ff-9f9271c2de88");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7d9d34a-337d-4bc8-b3e1-eb57b5e38099");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserMessages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9e6d5896-20b7-4750-b45f-f577f586c3f9", "1", "User", "User" },
                    { "b273c638-ed19-453d-8c67-2b91c3ae9570", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e6d5896-20b7-4750-b45f-f577f586c3f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b273c638-ed19-453d-8c67-2b91c3ae9570");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7409c9f4-4c5b-4517-a8ff-9f9271c2de88", "1", "Admin", "Admin" },
                    { "a7d9d34a-337d-4bc8-b3e1-eb57b5e38099", "1", "User", "User" }
                });
        }
    }
}

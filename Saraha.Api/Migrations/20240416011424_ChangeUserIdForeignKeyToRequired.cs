using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saraha.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdForeignKeyToRequired : Migration
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
                    { "78e19bef-5fa4-4d8a-b3f1-3ddb6384844b", "1", "Admin", "Admin" },
                    { "9edf1f04-ac3c-4b5f-a44e-b1acfc0addda", "1", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                keyValue: "78e19bef-5fa4-4d8a-b3f1-3ddb6384844b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9edf1f04-ac3c-4b5f-a44e-b1acfc0addda");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

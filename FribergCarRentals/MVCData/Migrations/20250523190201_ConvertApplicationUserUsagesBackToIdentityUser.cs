using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertApplicationUserUsagesBackToIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AspNetUsers_ApplicationUserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_ApplicationUserId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Customers",
                newName: "IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_ApplicationUserId",
                table: "Customers",
                newName: "IX_Customers_IdentityUserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Admins",
                newName: "IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_ApplicationUserId",
                table: "Admins",
                newName: "IX_Admins_IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AspNetUsers_IdentityUserId",
                table: "Admins",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_IdentityUserId",
                table: "Customers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AspNetUsers_IdentityUserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_IdentityUserId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Customers",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_IdentityUserId",
                table: "Customers",
                newName: "IX_Customers_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Admins",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_IdentityUserId",
                table: "Admins",
                newName: "IX_Admins_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AspNetUsers_ApplicationUserId",
                table: "Admins",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_ApplicationUserId",
                table: "Customers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

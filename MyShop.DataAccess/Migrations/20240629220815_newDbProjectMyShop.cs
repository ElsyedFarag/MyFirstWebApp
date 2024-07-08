using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newDbProjectMyShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeader_AspNetUsers_ApplicationUserId",
                table: "orderHeader");

            migrationBuilder.DropIndex(
                name: "IX_orderHeader_ApplicationUserId",
                table: "orderHeader");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "orderHeader");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUId",
                table: "orderHeader",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_orderHeader_ApplicationUId",
                table: "orderHeader",
                column: "ApplicationUId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeader_AspNetUsers_ApplicationUId",
                table: "orderHeader",
                column: "ApplicationUId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeader_AspNetUsers_ApplicationUId",
                table: "orderHeader");

            migrationBuilder.DropIndex(
                name: "IX_orderHeader_ApplicationUId",
                table: "orderHeader");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUId",
                table: "orderHeader",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "orderHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderHeader_ApplicationUserId",
                table: "orderHeader",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeader_AspNetUsers_ApplicationUserId",
                table: "orderHeader",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

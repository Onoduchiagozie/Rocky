using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Products",
                newName: "ApplicationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ApplicationId",
                table: "Products",
                newName: "IX_Products_ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ApplicationTypeId",
                table: "Products",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ApplicationTypeId",
                table: "Products",
                newName: "IX_Products_ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationId",
                table: "Products",
                column: "ApplicationId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedCustomerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_UserId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Sales",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_UserId",
                table: "Sales",
                newName: "IX_Sales_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_CustomerId",
                table: "Sales",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_CustomerId",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Sales",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_CustomerId",
                table: "Sales",
                newName: "IX_Sales_UserId");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Sales",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_UserId",
                table: "Sales",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

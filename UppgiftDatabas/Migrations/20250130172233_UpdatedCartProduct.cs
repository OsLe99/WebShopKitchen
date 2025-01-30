using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UppgiftDatabas.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCartProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_ShoppingCart_ShoppingCartId",
                table: "CartProduct");

            migrationBuilder.DropIndex(
                name: "IX_CartProduct_ShoppingCartId",
                table: "CartProduct");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "CartProduct");

            migrationBuilder.CreateIndex(
                name: "IX_CartProduct_CartId",
                table: "CartProduct",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_ShoppingCart_CartId",
                table: "CartProduct",
                column: "CartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_ShoppingCart_CartId",
                table: "CartProduct");

            migrationBuilder.DropIndex(
                name: "IX_CartProduct_CartId",
                table: "CartProduct");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "CartProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartProduct_ShoppingCartId",
                table: "CartProduct",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_ShoppingCart_ShoppingCartId",
                table: "CartProduct",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

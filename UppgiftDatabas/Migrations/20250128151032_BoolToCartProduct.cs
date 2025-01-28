using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UppgiftDatabas.Migrations
{
    /// <inheritdoc />
    public partial class BoolToCartProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "CartProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "CartProduct");
        }
    }
}

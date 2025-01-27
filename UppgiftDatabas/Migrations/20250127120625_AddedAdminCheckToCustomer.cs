using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UppgiftDatabas.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdminCheckToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Customer");
        }
    }
}

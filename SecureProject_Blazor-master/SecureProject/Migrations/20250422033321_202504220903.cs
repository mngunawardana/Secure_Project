using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureProject.Migrations
{
    /// <inheritdoc />
    public partial class _202504220903 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "VendorLogo",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorLogo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VendorName",
                table: "Products");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuzinShop.Migrations
{
    public partial class updateattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "Attributes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Attributes");
        }
    }
}

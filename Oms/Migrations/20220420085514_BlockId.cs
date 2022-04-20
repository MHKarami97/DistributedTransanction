using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oms.Migrations
{
    public partial class BlockId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlockCode",
                schema: "Oms",
                table: "Request",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockCode",
                schema: "Oms",
                table: "Request");
        }
    }
}

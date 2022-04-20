using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oms.Migrations
{
    public partial class OriginCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginCode",
                schema: "Oms",
                table: "Request",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginCode",
                schema: "Oms",
                table: "Request");
        }
    }
}

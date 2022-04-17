using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackOffice.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BackOffice");

            migrationBuilder.CreateTable(
                name: "DistributedTransactionModel",
                schema: "BackOffice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollaborationId = table.Column<int>(type: "int", nullable: false),
                    CommandType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommandBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributedTransactionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trade",
                schema: "BackOffice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradePrice = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TradedQuantity = table.Column<int>(type: "int", nullable: false),
                    InstrumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderSide = table.Column<int>(type: "int", nullable: false),
                    CollaborationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trade", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributedTransactionModel",
                schema: "BackOffice");

            migrationBuilder.DropTable(
                name: "Trade",
                schema: "BackOffice");
        }
    }
}

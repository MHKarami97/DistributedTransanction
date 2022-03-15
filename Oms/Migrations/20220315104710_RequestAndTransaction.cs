using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oms.Migrations
{
    public partial class RequestAndTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Oms");

            migrationBuilder.CreateTable(
                name: "Request",
                schema: "Oms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    RequestState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionalProcess",
                schema: "Oms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    TransactionState = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionalProcess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestError",
                schema: "Oms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestError_Request_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "Oms",
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestError_RequestId",
                schema: "Oms",
                table: "RequestError",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestError",
                schema: "Oms");

            migrationBuilder.DropTable(
                name: "TransactionalProcess",
                schema: "Oms");

            migrationBuilder.DropTable(
                name: "Request",
                schema: "Oms");
        }
    }
}

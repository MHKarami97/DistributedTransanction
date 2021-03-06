using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oms.Migrations
{
    public partial class TransactionStartDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                schema: "Oms",
                table: "DistributedTransactionModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDateTime",
                schema: "Oms",
                table: "DistributedTransactionModel");
        }
    }
}

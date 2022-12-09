using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kabuki_Desktop.Migrations
{
    public partial class AddDateTimeOrderInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeOrder",
                table: "Orders",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeOrder",
                table: "Orders");
        }
    }
}

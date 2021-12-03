using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TwitchAPI.Migrations
{
    public partial class timeSpans_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpiresIn",
                table: "Users",
                type: "time",
                nullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ExpiresIn",
                table: "Apps",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                table: "Users");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ExpiresIn",
                table: "Apps",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}

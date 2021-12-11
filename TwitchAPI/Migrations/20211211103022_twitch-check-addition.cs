using Microsoft.EntityFrameworkCore.Migrations;

namespace TwitchAPI.Migrations
{
    public partial class twitchcheckaddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConnectedTwitch",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectedTwitch",
                table: "AspNetUsers");
        }
    }
}

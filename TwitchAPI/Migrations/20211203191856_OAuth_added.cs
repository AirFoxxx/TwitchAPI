using Microsoft.EntityFrameworkCore.Migrations;

namespace TwitchAPI.Migrations
{
    public partial class OAuth_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OAuthCode",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OAuthCode",
                table: "Users");
        }
    }
}

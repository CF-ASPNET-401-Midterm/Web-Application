using Microsoft.EntityFrameworkCore.Migrations;

namespace thePlayList.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YouserEyeDee",
                table: "Playlists",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YouserEyeDee",
                table: "Playlists");
        }
    }
}

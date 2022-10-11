using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooNoteApp.Migrations
{
    public partial class newtable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Receiver_UserId",
                table: "CollabTable",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiver_UserId",
                table: "CollabTable");
        }
    }
}

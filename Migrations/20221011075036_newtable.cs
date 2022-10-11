using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooNoteApp.Migrations
{
    public partial class newtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTable_UserTable_UserId",
                table: "NoteTable");

            migrationBuilder.DropIndex(
                name: "IX_NoteTable_UserId",
                table: "NoteTable");

            migrationBuilder.CreateTable(
                name: "CollabTable",
                columns: table => new
                {
                    CollabId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender_UserId = table.Column<long>(nullable: false),
                    Receiver_Email = table.Column<string>(nullable: true),
                    NoteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollabTable", x => x.CollabId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollabTable");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTable_UserId",
                table: "NoteTable",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTable_UserTable_UserId",
                table: "NoteTable",
                column: "UserId",
                principalTable: "UserTable",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

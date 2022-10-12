using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooNoteApp.Migrations
{
    public partial class labeltable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelTable_NoteTable_NoteId",
                table: "LabelTable");

            migrationBuilder.DropForeignKey(
                name: "FK_LabelTable_UserTable_UserID",
                table: "LabelTable");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTable_UserTable_UserId",
                table: "NoteTable");

            migrationBuilder.DropIndex(
                name: "IX_NoteTable_UserId",
                table: "NoteTable");

            migrationBuilder.DropIndex(
                name: "IX_LabelTable_NoteId",
                table: "LabelTable");

            migrationBuilder.DropIndex(
                name: "IX_LabelTable_UserID",
                table: "LabelTable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NoteTable_UserId",
                table: "NoteTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelTable_NoteId",
                table: "LabelTable",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelTable_UserID",
                table: "LabelTable",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelTable_NoteTable_NoteId",
                table: "LabelTable",
                column: "NoteId",
                principalTable: "NoteTable",
                principalColumn: "NoteID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabelTable_UserTable_UserID",
                table: "LabelTable",
                column: "UserID",
                principalTable: "UserTable",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

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

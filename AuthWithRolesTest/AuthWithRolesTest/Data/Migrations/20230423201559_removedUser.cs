using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthWithRolesTest.Data.Migrations
{
    public partial class removedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotesModel_AspNetUsers_UserId",
                table: "NotesModel");

            migrationBuilder.DropIndex(
                name: "IX_NotesModel_UserId",
                table: "NotesModel");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NotesModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NotesModel",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NotesModel_UserId",
                table: "NotesModel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotesModel_AspNetUsers_UserId",
                table: "NotesModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

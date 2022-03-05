#nullable disable

namespace FitnessBuddy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedArticleAndArticleCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_AddedByUserId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "AddedByUserId",
                table: "Articles",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_AddedByUserId",
                table: "Articles",
                newName: "IX_Articles_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_CreatorId",
                table: "Articles",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_CreatorId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Articles",
                newName: "AddedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_CreatorId",
                table: "Articles",
                newName: "IX_Articles_AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AddedByUserId",
                table: "Articles",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

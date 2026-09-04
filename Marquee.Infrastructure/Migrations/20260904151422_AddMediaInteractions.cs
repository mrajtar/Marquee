using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marquee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaInteractions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaInteraction_AspNetUsers_UserId",
                table: "MediaInteraction");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaInteraction_Media_MediaId",
                table: "MediaInteraction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaInteraction",
                table: "MediaInteraction");

            migrationBuilder.RenameTable(
                name: "MediaInteraction",
                newName: "MediaInteractions");

            migrationBuilder.RenameIndex(
                name: "IX_MediaInteraction_UserId_MediaId_CreatedAt",
                table: "MediaInteractions",
                newName: "IX_MediaInteractions_UserId_MediaId_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_MediaInteraction_MediaId",
                table: "MediaInteractions",
                newName: "IX_MediaInteractions_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaInteraction_CreatedAt",
                table: "MediaInteractions",
                newName: "IX_MediaInteractions_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaInteractions",
                table: "MediaInteractions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaInteractions_AspNetUsers_UserId",
                table: "MediaInteractions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaInteractions_Media_MediaId",
                table: "MediaInteractions",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaInteractions_AspNetUsers_UserId",
                table: "MediaInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaInteractions_Media_MediaId",
                table: "MediaInteractions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaInteractions",
                table: "MediaInteractions");

            migrationBuilder.RenameTable(
                name: "MediaInteractions",
                newName: "MediaInteraction");

            migrationBuilder.RenameIndex(
                name: "IX_MediaInteractions_UserId_MediaId_CreatedAt",
                table: "MediaInteraction",
                newName: "IX_MediaInteraction_UserId_MediaId_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_MediaInteractions_MediaId",
                table: "MediaInteraction",
                newName: "IX_MediaInteraction_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaInteractions_CreatedAt",
                table: "MediaInteraction",
                newName: "IX_MediaInteraction_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaInteraction",
                table: "MediaInteraction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaInteraction_AspNetUsers_UserId",
                table: "MediaInteraction",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaInteraction_Media_MediaId",
                table: "MediaInteraction",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

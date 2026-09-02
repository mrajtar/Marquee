using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marquee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaList_AspNetUsers_UserId",
                table: "MediaList");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaListItem_MediaList_MediaListId",
                table: "MediaListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaListItem_Media_MediaId",
                table: "MediaListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaListItem",
                table: "MediaListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaList",
                table: "MediaList");

            migrationBuilder.RenameTable(
                name: "MediaListItem",
                newName: "MediaListItems");

            migrationBuilder.RenameTable(
                name: "MediaList",
                newName: "MediaLists");

            migrationBuilder.RenameIndex(
                name: "IX_MediaListItem_MediaListId_AddedAt",
                table: "MediaListItems",
                newName: "IX_MediaListItems_MediaListId_AddedAt");

            migrationBuilder.RenameIndex(
                name: "IX_MediaListItem_MediaId",
                table: "MediaListItems",
                newName: "IX_MediaListItems_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaList_UserId_Name",
                table: "MediaLists",
                newName: "IX_MediaLists_UserId_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaListItems",
                table: "MediaListItems",
                columns: new[] { "MediaListId", "MediaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaLists",
                table: "MediaLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaListItems_MediaLists_MediaListId",
                table: "MediaListItems",
                column: "MediaListId",
                principalTable: "MediaLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaListItems_Media_MediaId",
                table: "MediaListItems",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaLists_AspNetUsers_UserId",
                table: "MediaLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaListItems_MediaLists_MediaListId",
                table: "MediaListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaListItems_Media_MediaId",
                table: "MediaListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaLists_AspNetUsers_UserId",
                table: "MediaLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaLists",
                table: "MediaLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaListItems",
                table: "MediaListItems");

            migrationBuilder.RenameTable(
                name: "MediaLists",
                newName: "MediaList");

            migrationBuilder.RenameTable(
                name: "MediaListItems",
                newName: "MediaListItem");

            migrationBuilder.RenameIndex(
                name: "IX_MediaLists_UserId_Name",
                table: "MediaList",
                newName: "IX_MediaList_UserId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_MediaListItems_MediaListId_AddedAt",
                table: "MediaListItem",
                newName: "IX_MediaListItem_MediaListId_AddedAt");

            migrationBuilder.RenameIndex(
                name: "IX_MediaListItems_MediaId",
                table: "MediaListItem",
                newName: "IX_MediaListItem_MediaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaList",
                table: "MediaList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaListItem",
                table: "MediaListItem",
                columns: new[] { "MediaListId", "MediaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MediaList_AspNetUsers_UserId",
                table: "MediaList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaListItem_MediaList_MediaListId",
                table: "MediaListItem",
                column: "MediaListId",
                principalTable: "MediaList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaListItem_Media_MediaId",
                table: "MediaListItem",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

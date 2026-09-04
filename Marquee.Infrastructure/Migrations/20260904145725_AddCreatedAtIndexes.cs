using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marquee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReviewLikes_CreatedAt",
                table: "ReviewLikes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CreatedAt",
                table: "Ratings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MediaInteraction_CreatedAt",
                table: "MediaInteraction",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReviewLikes_CreatedAt",
                table: "ReviewLikes");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CreatedAt",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_MediaInteraction_CreatedAt",
                table: "MediaInteraction");
        }
    }
}

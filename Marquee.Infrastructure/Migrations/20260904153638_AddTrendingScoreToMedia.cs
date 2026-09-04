using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marquee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrendingScoreToMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TrendingScore",
                table: "Media",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrendingScore",
                table: "Media");
        }
    }
}

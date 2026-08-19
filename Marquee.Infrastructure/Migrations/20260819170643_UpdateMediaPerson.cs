using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marquee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMediaPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaPerson",
                table: "MediaPerson");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeathDate",
                table: "Person",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceOfBirth",
                table: "Person",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MediaPerson",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CreditOrder",
                table: "MediaPerson",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaPerson",
                table: "MediaPerson",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPerson_MediaId_PersonId_Role_CharacterName",
                table: "MediaPerson",
                columns: new[] { "MediaId", "PersonId", "Role", "CharacterName" },
                unique: true,
                filter: "[CharacterName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaPerson",
                table: "MediaPerson");

            migrationBuilder.DropIndex(
                name: "IX_MediaPerson_MediaId_PersonId_Role_CharacterName",
                table: "MediaPerson");

            migrationBuilder.DropColumn(
                name: "DeathDate",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PlaceOfBirth",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MediaPerson");

            migrationBuilder.DropColumn(
                name: "CreditOrder",
                table: "MediaPerson");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaPerson",
                table: "MediaPerson",
                columns: new[] { "MediaId", "PersonId", "Role" });
        }
    }
}

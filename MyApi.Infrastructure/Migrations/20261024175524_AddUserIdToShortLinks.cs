using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToShortLinks : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Skip dropping old PK and renaming — table is already named correctly in Neon
            // Just add the new column, FK and index

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ShortUrlTable",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ShortUrlTable_users_UserId",
                table: "ShortUrlTable",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrlTable_UserId",
                table: "ShortUrlTable",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortUrlTable_users_UserId",
                table: "ShortUrlTable");

            migrationBuilder.DropIndex(
                name: "IX_ShortUrlTable_UserId",
                table: "ShortUrlTable");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShortUrlTable");
        }
    }
}

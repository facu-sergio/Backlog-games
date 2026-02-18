using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BacklogGames.DataAccess.Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedAtToUserListGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "UserListGames",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "UserListGames");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BacklogGames.DataAccess.Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeToBeatFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletelySeconds",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HastilySeconds",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NormallySeconds",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeToBeatCount",
                table: "Games",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletelySeconds",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "HastilySeconds",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NormallySeconds",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TimeToBeatCount",
                table: "Games");
        }
    }
}

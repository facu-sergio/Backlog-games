using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BacklogGames.DataAccess.Layer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IgdbId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CoverUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FirstReleaseDate = table.Column<long>(type: "bigint", nullable: true),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Rating = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ColorHex = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserListGames",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    UserListId = table.Column<int>(type: "integer", nullable: false),
                    GameStatusId = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListGames", x => new { x.GameId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_UserListGames_GameStatus_GameStatusId",
                        column: x => x.GameStatusId,
                        principalTable: "GameStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserListGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListGames_UserList_UserListId",
                        column: x => x.UserListId,
                        principalTable: "UserList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameStatus",
                columns: new[] { "Id", "ColorHex", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Pendiente" },
                    { 2, null, null, "En Progreso" },
                    { 3, null, null, "Completado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_IgdbId",
                table: "Games",
                column: "IgdbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserListGames_GameStatusId",
                table: "UserListGames",
                column: "GameStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListGames_UserListId",
                table: "UserListGames",
                column: "UserListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserListGames");

            migrationBuilder.DropTable(
                name: "GameStatus");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "UserList");
        }
    }
}

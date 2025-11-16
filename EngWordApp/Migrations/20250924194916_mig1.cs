using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngWordApp.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EngWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuccessPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAsked = table.Column<bool>(type: "bit", nullable: false),
                    LastAsked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WordLevel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordId);
                });

            migrationBuilder.CreateTable(
                name: "Means",
                columns: table => new
                {
                    MeanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrMean = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Means", x => x.MeanId);
                    table.ForeignKey(
                        name: "FK_Means_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Means_WordId",
                table: "Means",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Means");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}

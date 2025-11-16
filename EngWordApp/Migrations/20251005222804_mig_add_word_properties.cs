using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngWordApp.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_word_properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AskCount",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Connective",
                table: "Words",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ScAnswerCount",
                table: "Words",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Weak",
                table: "Words",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WrongAnswerCount",
                table: "Words",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskCount",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Connective",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "ScAnswerCount",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Weak",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "WrongAnswerCount",
                table: "Words");
        }
    }
}

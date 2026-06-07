using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendeIA.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessModelType",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "How",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProjectType",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "What",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Why",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BreakEvenUnits",
                table: "ProjectFinancialAnalyses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "InternalRateOfReturn",
                table: "ProjectFinancialAnalyses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsViable",
                table: "ProjectFinancialAnalyses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "NetPresentValue",
                table: "ProjectFinancialAnalyses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessModelType",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "How",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectType",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "What",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Why",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BreakEvenUnits",
                table: "ProjectFinancialAnalyses");

            migrationBuilder.DropColumn(
                name: "InternalRateOfReturn",
                table: "ProjectFinancialAnalyses");

            migrationBuilder.DropColumn(
                name: "IsViable",
                table: "ProjectFinancialAnalyses");

            migrationBuilder.DropColumn(
                name: "NetPresentValue",
                table: "ProjectFinancialAnalyses");
        }
    }
}

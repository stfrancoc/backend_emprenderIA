using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendeIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntrepreneurProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Portfolio = table.Column<string>(type: "text", nullable: false),
                    Sector = table.Column<string>(type: "text", nullable: false),
                    ExperienceYears = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrepreneurProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrepreneurProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestorProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvestmentRangeMin = table.Column<decimal>(type: "numeric", nullable: false),
                    InvestmentRangeMax = table.Column<decimal>(type: "numeric", nullable: false),
                    Interests = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestorProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Specialties = table.Column<string>(type: "text", nullable: false),
                    YearsExperience = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntrepreneurProfiles_UserId",
                table: "EntrepreneurProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestorProfiles_UserId",
                table: "InvestorProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorProfiles_UserId",
                table: "MentorProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntrepreneurProfiles");

            migrationBuilder.DropTable(
                name: "InvestorProfiles");

            migrationBuilder.DropTable(
                name: "MentorProfiles");
        }
    }
}

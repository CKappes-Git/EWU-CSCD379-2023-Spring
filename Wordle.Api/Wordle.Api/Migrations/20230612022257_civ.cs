using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordle.Api.Migrations
{
    /// <inheritdoc />
    public partial class civ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CivAttributes",
                columns: table => new
                {
                    CivAttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivID = table.Column<int>(type: "int", nullable: false),
                    AttributeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbilityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivAttributes", x => x.CivAttributeID);
                });

            migrationBuilder.CreateTable(
                name: "Civs",
                columns: table => new
                {
                    CivID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Civs", x => x.CivID);
                });

            migrationBuilder.CreateTable(
                name: "LeaderAttributes",
                columns: table => new
                {
                    LeaderAttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaderID = table.Column<int>(type: "int", nullable: false),
                    AttributeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbilityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderAttributes", x => x.LeaderAttributeID);
                });

            migrationBuilder.CreateTable(
                name: "Leaders",
                columns: table => new
                {
                    LeaderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaders", x => x.LeaderID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CivAttributes");

            migrationBuilder.DropTable(
                name: "Civs");

            migrationBuilder.DropTable(
                name: "LeaderAttributes");

            migrationBuilder.DropTable(
                name: "Leaders");
        }
    }
}

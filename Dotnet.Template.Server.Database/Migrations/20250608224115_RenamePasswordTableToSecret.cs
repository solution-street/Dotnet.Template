using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.Template.Server.Database.Migrations;

/// <inheritdoc />
public partial class RenamePasswordTableToSecret : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Password", schema: "pass");

        migrationBuilder.EnsureSchema(name: "secret");

        migrationBuilder.CreateTable(
            name: "Secret",
            schema: "secret",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Ttl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Secret", x => x.Id);
            }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Secret", schema: "secret");

        migrationBuilder.EnsureSchema(name: "pass");

        migrationBuilder.CreateTable(
            name: "Password",
            schema: "pass",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Ttl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Password", x => x.Id);
            }
        );
    }
}

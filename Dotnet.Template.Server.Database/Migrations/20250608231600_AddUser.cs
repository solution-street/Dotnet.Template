using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.Template.Server.Database.Migrations;

/// <inheritdoc />
public partial class AddUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "users");

        migrationBuilder.AddColumn<Guid>(name: "UserId", schema: "secret", table: "Secret", type: "uniqueidentifier", nullable: true);

        migrationBuilder.CreateTable(
            name: "User",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AuthProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
            }
        );

        migrationBuilder.CreateIndex(name: "IX_Secret_UserId", schema: "secret", table: "Secret", column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_User_AuthProviderId",
            schema: "users",
            table: "User",
            column: "AuthProviderId",
            unique: true
        );

        migrationBuilder.AddForeignKey(
            name: "FK_Secret_User_UserId",
            schema: "secret",
            table: "Secret",
            column: "UserId",
            principalSchema: "users",
            principalTable: "User",
            principalColumn: "Id"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(name: "FK_Secret_User_UserId", schema: "secret", table: "Secret");

        migrationBuilder.DropTable(name: "User", schema: "users");

        migrationBuilder.DropIndex(name: "IX_Secret_UserId", schema: "secret", table: "Secret");

        migrationBuilder.DropColumn(name: "UserId", schema: "secret", table: "Secret");
    }
}

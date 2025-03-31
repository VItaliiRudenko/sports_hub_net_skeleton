using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsHub.Infrastructure.Db.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddJwtDenyList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jwt_deny_list",
                columns: table => new
                {
                    jti = table.Column<string>(type: "text", nullable: false),
                    iat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    exp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jwt_deny_list", x => x.jti);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jwt_deny_list");
        }
    }
}

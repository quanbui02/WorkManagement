using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Work.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class GrantedPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppGrantedPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermissionValue = table.Column<int>(type: "int", nullable: false),
                    UserIdGuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastModifyUserId = table.Column<int>(type: "int", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppControllerId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGrantedPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppGrantedPermissions_AppController_AppControllerId",
                        column: x => x.AppControllerId,
                        principalTable: "AppController",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppGrantedPermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppGrantedPermissions_AppControllerId",
                table: "AppGrantedPermissions",
                column: "AppControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGrantedPermissions_RoleId",
                table: "AppGrantedPermissions",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppGrantedPermissions");
        }
    }
}

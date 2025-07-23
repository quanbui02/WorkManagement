using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Work.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Fix_table_rolePermission_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionCode",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "RolePermission");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PermissionCode",
                table: "RolePermission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "RolePermission",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

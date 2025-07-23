using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Work.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Fix_table_rolePermission_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PermissionCode",
                table: "RolePermission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionCode",
                table: "RolePermission");
        }
    }
}

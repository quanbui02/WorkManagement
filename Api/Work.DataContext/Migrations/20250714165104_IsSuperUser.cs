﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Work.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class IsSuperUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperUser",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperUser",
                table: "AspNetUsers");
        }
    }
}

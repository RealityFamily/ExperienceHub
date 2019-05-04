using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VRTeleportator.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Caetegories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Caetegories",
                table: "Caetegories");

            migrationBuilder.RenameTable(
                name: "Caetegories",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Categories_CategoryId",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Categories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Caetegories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Caetegories",
                table: "Caetegories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Caetegories_CategoryId",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Caetegories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

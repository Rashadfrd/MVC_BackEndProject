using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Riode.Migrations
{
    public partial class ProductBadges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBadge_Badges_BadgeId",
                table: "ProductBadge");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBadge_Products_ProductId",
                table: "ProductBadge");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBadge",
                table: "ProductBadge");

            migrationBuilder.RenameTable(
                name: "ProductBadge",
                newName: "ProductBadges");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBadge_ProductId",
                table: "ProductBadges",
                newName: "IX_ProductBadges_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBadge_BadgeId",
                table: "ProductBadges",
                newName: "IX_ProductBadges_BadgeId");

            migrationBuilder.AlterColumn<int>(
                name: "MainCategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBadges",
                table: "ProductBadges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBadges_Badges_BadgeId",
                table: "ProductBadges",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBadges_Products_ProductId",
                table: "ProductBadges",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBadges_Badges_BadgeId",
                table: "ProductBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBadges_Products_ProductId",
                table: "ProductBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBadges",
                table: "ProductBadges");

            migrationBuilder.RenameTable(
                name: "ProductBadges",
                newName: "ProductBadge");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBadges_ProductId",
                table: "ProductBadge",
                newName: "IX_ProductBadge_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBadges_BadgeId",
                table: "ProductBadge",
                newName: "IX_ProductBadge_BadgeId");

            migrationBuilder.AlterColumn<int>(
                name: "MainCategoryId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBadge",
                table: "ProductBadge",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBadge_Badges_BadgeId",
                table: "ProductBadge",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBadge_Products_ProductId",
                table: "ProductBadge",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}

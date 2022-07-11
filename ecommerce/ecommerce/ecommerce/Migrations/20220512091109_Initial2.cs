using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "Sellers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SellerEmail",
                table: "Sellers",
                type: "char(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerPassword",
                table: "Sellers",
                type: "char(64)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerPhone",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banned",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerEmail",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerPassword",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerPhone",
                table: "Sellers");
        }
    }
}

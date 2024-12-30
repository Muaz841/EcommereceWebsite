﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailSender.Migrations
{
    /// <inheritdoc />
    public partial class productreviewfullauditadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_AbpUsers_UserId",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Products_ProductId",
                table: "ProductReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview");

            migrationBuilder.RenameTable(
                name: "ProductReview",
                newName: "ProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_UserId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_ProductId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AbpUsers_UserId",
                table: "ProductReviews",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AbpUsers_UserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.RenameTable(
                name: "ProductReviews",
                newName: "ProductReview");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_UserId",
                table: "ProductReview",
                newName: "IX_ProductReview_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReview",
                newName: "IX_ProductReview_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_AbpUsers_UserId",
                table: "ProductReview",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Products_ProductId",
                table: "ProductReview",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

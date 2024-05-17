using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptLearn.Modules.AccessControl.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_PermissionClaims_Type_Value",
                table: "UserClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_Users_UserId",
                table: "UserClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRefreshToken",
                table: "UserRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaim",
                table: "UserClaim");

            migrationBuilder.RenameTable(
                name: "UserRefreshToken",
                newName: "UserRefreshTokens");

            migrationBuilder.RenameTable(
                name: "UserClaim",
                newName: "UserClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshToken_UserId",
                table: "UserRefreshTokens",
                newName: "IX_UserRefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaim_Type_Value",
                table: "UserClaims",
                newName: "IX_UserClaims_Type_Value");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRefreshTokens",
                table: "UserRefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                columns: new[] { "UserId", "Type", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_PermissionClaims_Type_Value",
                table: "UserClaims",
                columns: new[] { "Type", "Value" },
                principalTable: "PermissionClaims",
                principalColumns: new[] { "Type", "Value" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshTokens_Users_UserId",
                table: "UserRefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_PermissionClaims_Type_Value",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_Users_UserId",
                table: "UserRefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRefreshTokens",
                table: "UserRefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserRefreshTokens",
                newName: "UserRefreshToken");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaim");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshToken",
                newName: "IX_UserRefreshToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaims_Type_Value",
                table: "UserClaim",
                newName: "IX_UserClaim_Type_Value");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRefreshToken",
                table: "UserRefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaim",
                table: "UserClaim",
                columns: new[] { "UserId", "Type", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_PermissionClaims_Type_Value",
                table: "UserClaim",
                columns: new[] { "Type", "Value" },
                principalTable: "PermissionClaims",
                principalColumns: new[] { "Type", "Value" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_Users_UserId",
                table: "UserClaim",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

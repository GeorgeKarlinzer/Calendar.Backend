using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptLearn.Modules.AccessControl.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_PermissionClaim_Type_Value",
                table: "UserClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionClaim",
                table: "PermissionClaim");

            migrationBuilder.RenameTable(
                name: "PermissionClaim",
                newName: "PermissionClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionClaims",
                table: "PermissionClaims",
                columns: new[] { "Type", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_PermissionClaims_Type_Value",
                table: "UserClaim",
                columns: new[] { "Type", "Value" },
                principalTable: "PermissionClaims",
                principalColumns: new[] { "Type", "Value" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_PermissionClaims_Type_Value",
                table: "UserClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionClaims",
                table: "PermissionClaims");

            migrationBuilder.RenameTable(
                name: "PermissionClaims",
                newName: "PermissionClaim");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionClaim",
                table: "PermissionClaim",
                columns: new[] { "Type", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_PermissionClaim_Type_Value",
                table: "UserClaim",
                columns: new[] { "Type", "Value" },
                principalTable: "PermissionClaim",
                principalColumns: new[] { "Type", "Value" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}

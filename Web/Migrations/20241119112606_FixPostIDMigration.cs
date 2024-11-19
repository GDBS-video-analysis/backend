using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class FixPostIDMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_posts_PostID1",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_PostID1",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "PostID1",
                table: "employees");

            migrationBuilder.AlterColumn<short>(
                name: "post_id",
                table: "employees",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_employees_post_id",
                table: "employees",
                column: "post_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_posts_post_id",
                table: "employees",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "post_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_posts_post_id",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_post_id",
                table: "employees");

            migrationBuilder.AlterColumn<long>(
                name: "post_id",
                table: "employees",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<short>(
                name: "PostID1",
                table: "employees",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_employees_PostID1",
                table: "employees",
                column: "PostID1");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_posts_PostID1",
                table: "employees",
                column: "PostID1",
                principalTable: "posts",
                principalColumn: "post_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class VideoAnalisysStatusObjectInEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_video_analisys_statuses_files_videofile_id",
                table: "video_analisys_statuses");

            migrationBuilder.DropIndex(
                name: "IX_video_analisys_statuses_videofile_id",
                table: "video_analisys_statuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_video_analisys_statuses_videofile_id",
                table: "video_analisys_statuses",
                column: "videofile_id");

            migrationBuilder.AddForeignKey(
                name: "FK_video_analisys_statuses_files_videofile_id",
                table: "video_analisys_statuses",
                column: "videofile_id",
                principalTable: "files",
                principalColumn: "file_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

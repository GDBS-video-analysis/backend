using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class VideoAnalisysStatusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "video_analisys_statuses",
                columns: table => new
                {
                    analisys_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    videofile_id = table.Column<long>(type: "bigint", nullable: false),
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video_analisys_statuses", x => x.analisys_id);
                    table.ForeignKey(
                        name: "FK_video_analisys_statuses_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_video_analisys_statuses_files_videofile_id",
                        column: x => x.videofile_id,
                        principalTable: "files",
                        principalColumn: "file_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_video_analisys_statuses_event_id",
                table: "video_analisys_statuses",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_video_analisys_statuses_videofile_id",
                table: "video_analisys_statuses",
                column: "videofile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "video_analisys_statuses");
        }
    }
}

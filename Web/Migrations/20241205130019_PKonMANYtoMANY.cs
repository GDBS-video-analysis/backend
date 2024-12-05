using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class PKonMANYtoMANY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "unregister_person_mark_id",
                table: "unregister_person_marks_events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "employee_mark_id",
                table: "employee_marks_events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_unregister_person_marks_events",
                table: "unregister_person_marks_events",
                column: "unregister_person_mark_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employee_marks_events",
                table: "employee_marks_events",
                column: "employee_mark_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_unregister_person_marks_events",
                table: "unregister_person_marks_events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employee_marks_events",
                table: "employee_marks_events");

            migrationBuilder.DropColumn(
                name: "unregister_person_mark_id",
                table: "unregister_person_marks_events");

            migrationBuilder.DropColumn(
                name: "employee_mark_id",
                table: "employee_marks_events");
        }
    }
}

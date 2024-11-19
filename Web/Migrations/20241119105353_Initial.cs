using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    department_id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(127)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.department_id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    event_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(127)", nullable: false),
                    date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    videofile = table.Column<string>(type: "character varying(1024)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "unregister_persons",
                columns: table => new
                {
                    unregister_person_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    biometrics = table.Column<string>(type: "character varying(1024)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unregister_persons", x => x.unregister_person_id);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    post_id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(127)", nullable: false),
                    department_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_posts_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "unregister_person_marks_events",
                columns: table => new
                {
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    unregister_person_id = table.Column<long>(type: "bigint", nullable: false),
                    videofile_mark = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_unregister_person_marks_events_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_unregister_person_marks_events_unregister_persons_unregiste~",
                        column: x => x.unregister_person_id,
                        principalTable: "unregister_persons",
                        principalColumn: "unregister_person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employee_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    firstname = table.Column<string>(type: "character varying(127)", nullable: false),
                    lastname = table.Column<string>(type: "character varying(127)", nullable: false),
                    patronymic = table.Column<string>(type: "character varying(127)", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(15)", nullable: false),
                    biometrics = table.Column<string>(type: "character varying(1024)", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    PostID1 = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employees_posts_PostID1",
                        column: x => x.PostID1,
                        principalTable: "posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_marks_events",
                columns: table => new
                {
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    videofile_mark = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_employee_marks_events_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_marks_events_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expected_employee_events",
                columns: table => new
                {
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    events_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expected_employee_events", x => new { x.employee_id, x.events_id });
                    table.ForeignKey(
                        name: "FK_expected_employee_events_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expected_employee_events_events_events_id",
                        column: x => x.events_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    login = table.Column<string>(type: "character varying (127)", nullable: false),
                    password = table.Column<string>(type: "character varying(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_marks_events_employee_id",
                table: "employee_marks_events",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_marks_events_event_id",
                table: "employee_marks_events",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_expected_employee_events_events_id",
                table: "expected_employee_events",
                column: "events_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_PostID1",
                table: "employees",
                column: "PostID1");

            migrationBuilder.CreateIndex(
                name: "IX_posts_department_id",
                table: "posts",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_unregister_person_marks_events_event_id",
                table: "unregister_person_marks_events",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_unregister_person_marks_events_unregister_person_id",
                table: "unregister_person_marks_events",
                column: "unregister_person_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_employee_id",
                table: "users",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_marks_events");

            migrationBuilder.DropTable(
                name: "expected_employee_events");

            migrationBuilder.DropTable(
                name: "unregister_person_marks_events");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "unregister_persons");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}

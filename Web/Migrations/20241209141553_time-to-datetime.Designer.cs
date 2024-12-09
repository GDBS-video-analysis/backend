﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web.DataBaseContext;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(VideoAnalisysDBContext))]
    [Migration("20241209141553_time-to-datetime")]
    partial class timetodatetime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmployeeEvent", b =>
                {
                    b.Property<long>("ExpectedEmployeesEmployeeID")
                        .HasColumnType("bigint");

                    b.Property<long>("ExpectedEventsEventID")
                        .HasColumnType("bigint");

                    b.HasKey("ExpectedEmployeesEmployeeID", "ExpectedEventsEventID");

                    b.HasIndex("ExpectedEventsEventID");

                    b.ToTable("EmployeeEvent");
                });

            modelBuilder.Entity("EmployeeMinioFile", b =>
                {
                    b.Property<long>("BiometricsFileID")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployeesEmployeeID")
                        .HasColumnType("bigint");

                    b.HasKey("BiometricsFileID", "EmployeesEmployeeID");

                    b.HasIndex("EmployeesEmployeeID");

                    b.ToTable("EmployeeMinioFile");
                });

            modelBuilder.Entity("Web.Entities.Department", b =>
                {
                    b.Property<short>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("department_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<short>("DepartmentID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("name");

                    b.HasKey("DepartmentID");

                    b.ToTable("departments");
                });

            modelBuilder.Entity("Web.Entities.Employee", b =>
                {
                    b.Property<long>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("employee_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("EmployeeID"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("firstname");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("lastname");

                    b.Property<string>("Patronymic")
                        .HasColumnType("character varying(127)")
                        .HasColumnName("patronymic");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("character varying(15)")
                        .HasColumnName("phone_number");

                    b.Property<short>("PostID")
                        .HasColumnType("smallint")
                        .HasColumnName("post_id");

                    b.HasKey("EmployeeID");

                    b.HasIndex("PostID");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("Web.Entities.EmployeeMarksEvents", b =>
                {
                    b.Property<long>("EmployeeMarkID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("employee_mark_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("EmployeeMarkID"));

                    b.Property<long>("EmployeeID")
                        .HasColumnType("bigint")
                        .HasColumnName("employee_id");

                    b.Property<long>("EventID")
                        .HasColumnType("bigint")
                        .HasColumnName("event_id");

                    b.Property<DateTime>("VideoFileMark")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("videofile_mark");

                    b.HasKey("EmployeeMarkID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("EventID");

                    b.ToTable("employee_marks_events");
                });

            modelBuilder.Entity("Web.Entities.Event", b =>
                {
                    b.Property<long>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("event_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("EventID"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_time");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("name");

                    b.Property<long?>("VideoFileID")
                        .HasColumnType("bigint")
                        .HasColumnName("videofile_id");

                    b.HasKey("EventID");

                    b.HasIndex("VideoFileID");

                    b.ToTable("events");
                });

            modelBuilder.Entity("Web.Entities.MinioFile", b =>
                {
                    b.Property<long>("FileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("file_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("FileID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("mimetype");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("path");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.HasKey("FileID");

                    b.ToTable("files");
                });

            modelBuilder.Entity("Web.Entities.Post", b =>
                {
                    b.Property<short>("PostID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("post_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<short>("PostID"));

                    b.Property<short>("DepartmentID")
                        .HasColumnType("smallint")
                        .HasColumnName("department_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(127)")
                        .HasColumnName("name");

                    b.HasKey("PostID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("posts");
                });

            modelBuilder.Entity("Web.Entities.UnregisterPersonMarksEvents", b =>
                {
                    b.Property<long>("UnregisterPersonMarkID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("unregister_person_mark_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UnregisterPersonMarkID"));

                    b.Property<long>("EventID")
                        .HasColumnType("bigint")
                        .HasColumnName("event_id");

                    b.Property<long>("UnregisterPersonID")
                        .HasColumnType("bigserial")
                        .HasColumnName("unregister_person_id");

                    b.Property<DateTime>("VideoFileMark")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("videofile_mark");

                    b.Property<long>("VideoFragmentID")
                        .HasColumnType("bigint")
                        .HasColumnName("videofile_fragment_id");

                    b.HasKey("UnregisterPersonMarkID");

                    b.HasIndex("EventID");

                    b.HasIndex("VideoFragmentID");

                    b.ToTable("unregister_person_marks_events");
                });

            modelBuilder.Entity("Web.Entities.User", b =>
                {
                    b.Property<long>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserID"));

                    b.Property<long>("EmployeeID")
                        .HasColumnType("bigint")
                        .HasColumnName("employee_id");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("character varying (127)")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.HasKey("UserID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Web.Entities.VideoAnalisysStatus", b =>
                {
                    b.Property<long>("AnalisysID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("analisys_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("AnalisysID"));

                    b.Property<long>("EventID")
                        .HasColumnType("bigint")
                        .HasColumnName("event_id");

                    b.Property<short>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("status");

                    b.Property<long>("VideoFileID")
                        .HasColumnType("bigint")
                        .HasColumnName("videofile_id");

                    b.HasKey("AnalisysID");

                    b.HasIndex("EventID");

                    b.ToTable("video_analisys_statuses");
                });

            modelBuilder.Entity("EmployeeEvent", b =>
                {
                    b.HasOne("Web.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("ExpectedEmployeesEmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("ExpectedEventsEventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeMinioFile", b =>
                {
                    b.HasOne("Web.Entities.MinioFile", null)
                        .WithMany()
                        .HasForeignKey("BiometricsFileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesEmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web.Entities.Employee", b =>
                {
                    b.HasOne("Web.Entities.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Web.Entities.EmployeeMarksEvents", b =>
                {
                    b.HasOne("Web.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Web.Entities.Event", b =>
                {
                    b.HasOne("Web.Entities.MinioFile", "VideoFile")
                        .WithMany()
                        .HasForeignKey("VideoFileID");

                    b.Navigation("VideoFile");
                });

            modelBuilder.Entity("Web.Entities.Post", b =>
                {
                    b.HasOne("Web.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Web.Entities.UnregisterPersonMarksEvents", b =>
                {
                    b.HasOne("Web.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.MinioFile", "VideoFragment")
                        .WithMany()
                        .HasForeignKey("VideoFragmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("VideoFragment");
                });

            modelBuilder.Entity("Web.Entities.User", b =>
                {
                    b.HasOne("Web.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Web.Entities.VideoAnalisysStatus", b =>
                {
                    b.HasOne("Web.Entities.Event", null)
                        .WithMany("VideoAnalisysStatus")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web.Entities.Event", b =>
                {
                    b.Navigation("VideoAnalisysStatus");
                });
#pragma warning restore 612, 618
        }
    }
}

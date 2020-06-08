﻿// <auto-generated />
using CustomTrackerBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CustomTrackerBackend.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20200526052714_ManPass")]
    partial class ManPass
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            #pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CustomTrackerBackend.Models.Issue", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .HasColumnType("bigint")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<bool>("IsComplete")
                    .HasColumnName("is_complete")
                    .HasColumnType("boolean");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                b.Property<string>("UserId")
                    .HasColumnName("user_id")
                    .HasColumnType("text");

                b.HasKey("Id")
                    .HasName("pk_issues");

                b.HasIndex("UserId")
                    .HasName("ix_issues_user_id");

                b.ToTable("issues");
            });

            modelBuilder.Entity("CustomTrackerBackend.Models.User", b =>
            {
                b.Property<string>("Id")
                    .HasColumnName("id")
                    .HasColumnType("text");

                b.Property<string>("PasswordHash")
                    .HasColumnName("password_hash")
                    .HasColumnType("text");

                b.Property<string>("Username")
                    .HasColumnName("username")
                    .HasColumnType("text");

                b.HasKey("Id")
                    .HasName("pk_users");

                b.HasIndex("Username")
                    .IsUnique()
                    .HasName("ix_users_username");

                b.ToTable("users");
            });

            modelBuilder.Entity("CustomTrackerBackend.Models.Issue", b =>
            {
                b.HasOne("CustomTrackerBackend.Models.User", "User")
                    .WithMany("Issues")
                    .HasForeignKey("UserId")
                    .HasConstraintName("fk_issues_users_user_id");
            });
            #pragma warning restore 612, 618
        }
    }
}

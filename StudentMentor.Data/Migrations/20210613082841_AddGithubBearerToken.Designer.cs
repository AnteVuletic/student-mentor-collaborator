﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentMentor.Data.Entities;

namespace StudentMentor.Data.Migrations
{
    [DbContext(typeof(StudentMentorDbContext))]
    [Migration("20210613082841_AddGithubBearerToken")]
    partial class AddGithubBearerToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MessageCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserFromId")
                        .HasColumnType("int");

                    b.Property<int?>("UserToId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserFromId");

                    b.HasIndex("UserToId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<int>("UserRole");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Admin", b =>
                {
                    b.HasBaseType("StudentMentor.Data.Entities.Models.User");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Mentor", b =>
                {
                    b.HasBaseType("StudentMentor.Data.Entities.Models.User");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Student", b =>
                {
                    b.HasBaseType("StudentMentor.Data.Entities.Models.User");

                    b.Property<string>("GithubAccessKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GithubBearerToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MentorId")
                        .HasColumnType("int");

                    b.HasIndex("GithubAccessKey")
                        .IsUnique()
                        .HasFilter("[GithubAccessKey] IS NOT NULL");

                    b.HasIndex("MentorId");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Message", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.User", "UserFrom")
                        .WithMany("MessagesSent")
                        .HasForeignKey("UserFromId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("StudentMentor.Data.Entities.Models.User", "UserTo")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("UserToId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("UserFrom");

                    b.Navigation("UserTo");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Student", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.Mentor", "Mentor")
                        .WithMany("Students")
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Mentor");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.User", b =>
                {
                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Mentor", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}

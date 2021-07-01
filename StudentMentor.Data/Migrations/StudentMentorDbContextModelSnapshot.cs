﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentMentor.Data.Entities;

namespace StudentMentor.Data.Migrations
{
    [DbContext(typeof(StudentMentorDbContext))]
    partial class StudentMentorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.Commit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PushActivityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PushActivityId");

                    b.ToTable("Commits", "github");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.FileLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChangeType")
                        .HasColumnType("int");

                    b.Property<string>("CommitId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("File")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CommitId");

                    b.ToTable("FileLogs", "github");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.PushActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ref")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.Property<string>("RepositoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.ToTable("PushActivities", "github");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MessageCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PushActivityId")
                        .HasColumnType("int");

                    b.Property<int?>("UserFromId")
                        .HasColumnType("int");

                    b.Property<int?>("UserToId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileId")
                        .IsUnique()
                        .HasFilter("[FileId] IS NOT NULL");

                    b.HasIndex("PushActivityId")
                        .IsUnique()
                        .HasFilter("[PushActivityId] IS NOT NULL");

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

            modelBuilder.Entity("StudentMentor.Data.Entities.StudentFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FileId")
                        .IsUnique();

                    b.HasIndex("StudentId");

                    b.ToTable("StudentFiles");
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

                    b.Property<int>("GithubRepositoryId")
                        .HasColumnType("int");

                    b.Property<int?>("MentorId")
                        .HasColumnType("int");

                    b.HasIndex("GithubAccessKey")
                        .IsUnique()
                        .HasFilter("[GithubAccessKey] IS NOT NULL");

                    b.HasIndex("MentorId");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Comment", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.Message", "Message")
                        .WithMany("Comments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentMentor.Data.Entities.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.Commit", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.Github.PushActivity", "PushActivity")
                        .WithMany("Commits")
                        .HasForeignKey("PushActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PushActivity");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.FileLog", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.Github.Commit", "Commit")
                        .WithMany("FileLogs")
                        .HasForeignKey("CommitId");

                    b.Navigation("Commit");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Message", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.File", "File")
                        .WithOne("Message")
                        .HasForeignKey("StudentMentor.Data.Entities.Models.Message", "FileId");

                    b.HasOne("StudentMentor.Data.Entities.Models.Github.PushActivity", "PushActivity")
                        .WithOne("Message")
                        .HasForeignKey("StudentMentor.Data.Entities.Models.Message", "PushActivityId");

                    b.HasOne("StudentMentor.Data.Entities.Models.User", "UserFrom")
                        .WithMany("MessagesSent")
                        .HasForeignKey("UserFromId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("StudentMentor.Data.Entities.Models.User", "UserTo")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("UserToId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("File");

                    b.Navigation("PushActivity");

                    b.Navigation("UserFrom");

                    b.Navigation("UserTo");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.StudentFile", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.File", "File")
                        .WithOne("StudentFile")
                        .HasForeignKey("StudentMentor.Data.Entities.StudentFile", "FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentMentor.Data.Entities.Models.Student", "Student")
                        .WithMany("FinalPapers")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Student", b =>
                {
                    b.HasOne("StudentMentor.Data.Entities.Models.Mentor", "Mentor")
                        .WithMany("Students")
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Mentor");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.File", b =>
                {
                    b.Navigation("Message");

                    b.Navigation("StudentFile");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.Commit", b =>
                {
                    b.Navigation("FileLogs");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Github.PushActivity", b =>
                {
                    b.Navigation("Commits");

                    b.Navigation("Message");
                });

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Message", b =>
                {
                    b.Navigation("Comments");
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

            modelBuilder.Entity("StudentMentor.Data.Entities.Models.Student", b =>
                {
                    b.Navigation("FinalPapers");
                });
#pragma warning restore 612, 618
        }
    }
}

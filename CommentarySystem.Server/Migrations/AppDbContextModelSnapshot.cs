﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Data;

#nullable disable

namespace CommentarySystem.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("comment_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("parent_comment_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("text");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.HasKey("CommentId")
                        .HasName("pk_comment");

                    b.HasIndex("ParentCommentId")
                        .HasDatabaseName("ix_comment_parent_comment_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_comment_user_id");

                    b.ToTable("comment", (string)null);
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("file_id");

                    b.Property<int>("CommentId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("comment_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("content");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("file_name");

                    b.Property<long>("FileSize")
                        .HasColumnType("INTEGER")
                        .HasColumnName("file_size");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT")
                        .HasColumnName("file_type");

                    b.HasKey("FileId")
                        .HasName("pk_file");

                    b.HasIndex("CommentId")
                        .HasDatabaseName("ix_file_comment_id");

                    b.ToTable("file", (string)null);
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.SelectedRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("CommentId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("comment_id");

                    b.Property<int>("EndIndex")
                        .HasColumnType("INTEGER")
                        .HasColumnName("end_index");

                    b.Property<int>("StartIndex")
                        .HasColumnType("INTEGER")
                        .HasColumnName("start_index");

                    b.HasKey("Id")
                        .HasName("pk_selected_range");

                    b.HasIndex("CommentId")
                        .HasDatabaseName("ix_selected_range_comment_id");

                    b.ToTable("selected_range", (string)null);
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("user_name");

                    b.HasKey("UserId")
                        .HasName("pk_user");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.Comment", b =>
                {
                    b.HasOne("CommentarySystem.Server.Data.Entities.Comment", "ParentComment")
                        .WithMany("ChildComments")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_comment_comment_parent_comment_id");

                    b.HasOne("CommentarySystem.Server.Data.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comment_user_user_id");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.File", b =>
                {
                    b.HasOne("CommentarySystem.Server.Data.Entities.Comment", "Comment")
                        .WithMany("Files")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_file_comment_comment_id");

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.SelectedRange", b =>
                {
                    b.HasOne("CommentarySystem.Server.Data.Entities.Comment", "Comment")
                        .WithMany("SelectedRandges")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_selected_range_comment_comment_id");

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.Comment", b =>
                {
                    b.Navigation("ChildComments");

                    b.Navigation("Files");

                    b.Navigation("SelectedRandges");
                });

            modelBuilder.Entity("CommentarySystem.Server.Data.Entities.User", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}

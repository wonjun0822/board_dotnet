﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using board_dotnet.Data;

#nullable disable

namespace boarddotnet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230130000331_ArticleToMemberRelationship")]
    partial class ArticleToMemberRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("board_dotnet.Model.Article", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_at");

                    b.Property<string>("createBy")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("create_by");

                    b.Property<string>("createby")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("hashTag")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("hash_tag");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("updateAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_at");

                    b.Property<string>("updateBy")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("update_by");

                    b.Property<int>("viewCount")
                        .HasColumnType("int")
                        .HasColumnName("view_count");

                    b.HasKey("id");

                    b.HasIndex("createAt");

                    b.HasIndex("createBy");

                    b.HasIndex("createby");

                    b.HasIndex("title");

                    b.ToTable("article");
                });

            modelBuilder.Entity("board_dotnet.Model.Comment", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("articleId")
                        .HasColumnType("bigint")
                        .HasColumnName("article_id");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_at");

                    b.Property<string>("createBy")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("create_by");

                    b.Property<DateTime>("updateAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_at");

                    b.Property<string>("updateBy")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("update_by");

                    b.HasKey("id");

                    b.HasIndex("articleId");

                    b.HasIndex("comment");

                    b.HasIndex("createAt");

                    b.HasIndex("createBy");

                    b.ToTable("comment");
                });

            modelBuilder.Entity("board_dotnet.Model.Member", b =>
                {
                    b.Property<string>("member_id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("nickname")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("pwd")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("member_id");

                    b.ToTable("member");
                });

            modelBuilder.Entity("board_dotnet.Model.Article", b =>
                {
                    b.HasOne("board_dotnet.Model.Member", null)
                        .WithMany()
                        .HasForeignKey("createBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("board_dotnet.Model.Member", "member")
                        .WithMany()
                        .HasForeignKey("createby");

                    b.Navigation("member");
                });

            modelBuilder.Entity("board_dotnet.Model.Comment", b =>
                {
                    b.HasOne("board_dotnet.Model.Article", null)
                        .WithMany("articleComments")
                        .HasForeignKey("articleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("board_dotnet.Model.Article", b =>
                {
                    b.Navigation("articleComments");
                });
#pragma warning restore 612, 618
        }
    }
}

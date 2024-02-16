﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepoRanger.Persistence;

#nullable disable

namespace RepoRanger.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240216034931_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("BranchProject", b =>
                {
                    b.Property<Guid>("BranchesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("TEXT");

                    b.HasKey("BranchesId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("BranchProject");
                });

            modelBuilder.Entity("DependencyProject", b =>
                {
                    b.Property<Guid>("DependenciesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("TEXT");

                    b.HasKey("DependenciesId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("DependencyProject");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Branch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DefaultRepositoryId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DefaultRepositoryId")
                        .IsUnique();

                    b.HasIndex("RepositoryId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Dependency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RepositoryId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SourceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.HasIndex("SourceId");

                    b.ToTable("Dependencies");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Repository", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DefaultBranchId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RemoteUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("BranchProject", b =>
                {
                    b.HasOne("RepoRanger.Domain.Source.Branch", null)
                        .WithMany()
                        .HasForeignKey("BranchesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepoRanger.Domain.Source.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DependencyProject", b =>
                {
                    b.HasOne("RepoRanger.Domain.Source.Dependency", null)
                        .WithMany()
                        .HasForeignKey("DependenciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepoRanger.Domain.Source.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Branch", b =>
                {
                    b.HasOne("RepoRanger.Domain.Source.Repository", null)
                        .WithOne("DefaultBranch")
                        .HasForeignKey("RepoRanger.Domain.Source.Branch", "DefaultRepositoryId");

                    b.HasOne("RepoRanger.Domain.Source.Repository", null)
                        .WithMany("Branches")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Dependency", b =>
                {
                    b.HasOne("RepoRanger.Domain.Source.Repository", null)
                        .WithMany("Dependencies")
                        .HasForeignKey("RepositoryId");

                    b.HasOne("RepoRanger.Domain.Source.Source", null)
                        .WithMany("Dependencies")
                        .HasForeignKey("SourceId");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Repository", b =>
                {
                    b.HasOne("RepoRanger.Domain.Source.Source", "Source")
                        .WithMany("Repositories")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Repository", b =>
                {
                    b.Navigation("Branches");

                    b.Navigation("DefaultBranch")
                        .IsRequired();

                    b.Navigation("Dependencies");
                });

            modelBuilder.Entity("RepoRanger.Domain.Source.Source", b =>
                {
                    b.Navigation("Dependencies");

                    b.Navigation("Repositories");
                });
#pragma warning restore 612, 618
        }
    }
}
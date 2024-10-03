﻿// <auto-generated />
using System;
using EFModels.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFModels.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241003150318_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EFModels.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ISBN")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("books");
                });
#pragma warning restore 612, 618
        }
    }
}

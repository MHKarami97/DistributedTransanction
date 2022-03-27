﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oms.Context;

#nullable disable

namespace Oms.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220326125744_collaborationGenerator")]
    partial class collaborationGenerator
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Oms.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RequestState")
                        .HasColumnType("int");

                    b.Property<int>("RequestType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Request", "Oms");
                });

            modelBuilder.Entity("Saga.V2.DistributedTransactionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CollaborationId")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("[Id]");

                    b.Property<string>("CommandBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommandType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<byte>("State")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("DistributedTransactionModel", "Oms");
                });

            modelBuilder.Entity("Oms.Models.Request", b =>
                {
                    b.OwnsMany("Oms.Models.RequestError", "Errors", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<string>("Message")
                                .HasMaxLength(512)
                                .HasColumnType("nvarchar(512)");

                            b1.Property<int>("RequestId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("RequestId");

                            b1.ToTable("RequestError", "Oms");

                            b1.WithOwner()
                                .HasForeignKey("RequestId");
                        });

                    b.Navigation("Errors");
                });
#pragma warning restore 612, 618
        }
    }
}
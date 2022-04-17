﻿// <auto-generated />
using System;
using BackOffice.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackOffice.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BackOffice.Models.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CollaborationId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("InstrumentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderSide")
                        .HasColumnType("int");

                    b.Property<long>("TradePrice")
                        .HasColumnType("bigint");

                    b.Property<int>("TradedQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Trade", "BackOffice");
                });

            modelBuilder.Entity("Saga.V2.DistributedTransactionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CollaborationId")
                        .HasColumnType("int");

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

                    b.ToTable("DistributedTransactionModel", "BackOffice");
                });
#pragma warning restore 612, 618
        }
    }
}
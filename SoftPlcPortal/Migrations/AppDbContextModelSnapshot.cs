﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoftPlcPortal.Infrastructure.Database;

#nullable disable

namespace SoftPlcPortal.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.DataBlock", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlcConfigKey")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("PlcConfigKey", "Number")
                        .IsUnique();

                    b.ToTable("DataBlocks");
                });

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.DbField", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("BitOffset")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ByteOffset")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DataBlockKey")
                        .HasColumnType("TEXT");

                    b.Property<int>("DataType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StartValue")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("DataBlockKey");

                    b.ToTable("DbFields");
                });

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.PlcConfig", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ApiPort")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PlcPort")
                        .HasColumnType("INTEGER");

                    b.HasKey("Key");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Address", "PlcPort")
                        .IsUnique();

                    b.ToTable("PlcConfigs");
                });

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.DataBlock", b =>
                {
                    b.HasOne("SoftPlcPortal.Infrastructure.Tables.PlcConfig", "PlcConfig")
                        .WithMany("DataBlocks")
                        .HasForeignKey("PlcConfigKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlcConfig");
                });

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.DbField", b =>
                {
                    b.HasOne("SoftPlcPortal.Infrastructure.Tables.DataBlock", "DataBlock")
                        .WithMany("DbFields")
                        .HasForeignKey("DataBlockKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataBlock");
                });

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.DataBlock", b =>
                {
                    b.Navigation("DbFields");
                });

            modelBuilder.Entity("SoftPlcPortal.Infrastructure.Tables.PlcConfig", b =>
                {
                    b.Navigation("DataBlocks");
                });
#pragma warning restore 612, 618
        }
    }
}

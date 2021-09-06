﻿// <auto-generated />

using System;
using Argus.API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#pragma warning disable CS1591

namespace Argus.API.Migrations
{
    [DbContext(typeof(ArgusAPIContext))]
    partial class ArgusAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("Argus.API.Database.Model.APIKey", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Key")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ID")
                        .IsUnique();

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("APIKeys");
                });
#pragma warning restore 612, 618
        }
    }
}

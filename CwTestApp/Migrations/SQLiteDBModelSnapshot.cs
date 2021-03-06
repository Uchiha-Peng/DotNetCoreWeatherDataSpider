﻿// <auto-generated />
using System;
using CwTestApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CwTestApp.Migrations
{
    [DbContext(typeof(SQLiteDB))]
    partial class SQLiteDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity("CwTestApp.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CityEnglishName");

                    b.Property<string>("CityName");

                    b.HasKey("ID");

                    b.ToTable("Citys");
                });

            modelBuilder.Entity("CwTestApp.Models.Weather", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CityID");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Temperature");

                    b.Property<string>("WeatherDetail");

                    b.Property<string>("Wind");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("Weathers");
                });

            modelBuilder.Entity("CwTestApp.Models.Weather", b =>
                {
                    b.HasOne("CwTestApp.Models.City")
                        .WithMany("Weathers")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

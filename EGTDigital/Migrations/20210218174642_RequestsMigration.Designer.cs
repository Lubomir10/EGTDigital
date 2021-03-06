﻿// <auto-generated />
using EGTDigital.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EGTDigital.Migrations
{
    [DbContext(typeof(EgtDbContext))]
    [Migration("20210218174642_RequestsMigration")]
    partial class RequestsMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("EGTDigital.Entities.Request", b =>
                {
                    b.Property<string>("RequestId")
                        .HasColumnType("text");

                    b.HasKey("RequestId");

                    b.ToTable("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}

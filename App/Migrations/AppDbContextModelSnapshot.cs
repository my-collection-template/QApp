﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using QApp.Database;

namespace QApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("QApp.Database.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedUtc");

                    b.Property<string>("Email");

                    b.Property<bool>("Enabled");

                    b.Property<bool>("IsAdmin");

                    b.Property<DateTimeOffset>("ModifiedUtc");

                    b.Property<string>("Name");

                    b.Property<string>("PasswordHash");

                    b.Property<Guid>("PasswordSalt");

                    b.Property<bool>("ResetPassword");

                    b.Property<string>("SessionToken");

                    b.Property<DateTimeOffset>("SessionTokenExpiresUtc");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}

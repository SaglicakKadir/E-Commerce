﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ecommerce.Areas.Admin.Models;

#nullable disable

namespace ecommerce.Migrations.User
{
    [DbContext(typeof(UserContext))]
    [Migration("20220602105726_er")]
    partial class er
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ecommerce.Areas.Admin.Models.User", b =>
                {
                    b.Property<short>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("UserId"), 1L, 1);

                    b.Property<bool>("CreateCategory")
                        .HasColumnType("bit");

                    b.Property<bool>("CreateSeller")
                        .HasColumnType("bit");

                    b.Property<bool>("CreateUser")
                        .HasColumnType("bit");

                    b.Property<bool>("DeleteCategory")
                        .HasColumnType("bit");

                    b.Property<bool>("DeleteProduct")
                        .HasColumnType("bit");

                    b.Property<bool>("DeleteSeller")
                        .HasColumnType("bit");

                    b.Property<bool>("DeleteUser")
                        .HasColumnType("bit");

                    b.Property<bool>("EditCategory")
                        .HasColumnType("bit");

                    b.Property<bool>("EditProduct")
                        .HasColumnType("bit");

                    b.Property<bool>("EditSeller")
                        .HasColumnType("bit");

                    b.Property<bool>("EditUser")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("char(100)");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("char(64)");

                    b.Property<bool>("ViewCategories")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewSellers")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewUsers")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
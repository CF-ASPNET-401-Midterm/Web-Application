﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using thePlayList.Data;

namespace thePlayList.Migrations
{
    [DbContext(typeof(MusicDbContext))]
    [Migration("20180706010936_initialthree")]
    partial class initialthree
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("thePlayList.Models.Playlist", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GenreID");

                    b.Property<string>("Name");

                    b.Property<int>("YouserEyeDee");

                    b.HasKey("Id");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("thePlayList.Models.Song", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Album");

                    b.Property<string>("Artist");

                    b.Property<int>("DatListEyeDee");

                    b.Property<string>("Genre");

                    b.Property<string>("Name");

                    b.Property<int?>("PlaylistId");

                    b.Property<DateTime?>("ReleaseDate");

                    b.HasKey("ID");

                    b.HasIndex("PlaylistId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("thePlayList.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DatGenreEyeDee");

                    b.Property<int>("DatListEyeDee");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("thePlayList.Models.Song", b =>
                {
                    b.HasOne("thePlayList.Models.Playlist")
                        .WithMany("Songs")
                        .HasForeignKey("PlaylistId");
                });
#pragma warning restore 612, 618
        }
    }
}

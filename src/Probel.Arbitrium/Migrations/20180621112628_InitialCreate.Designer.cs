﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Probel.Arbitrium.Models;
using System;

namespace Probel.Arbitrium.Migrations
{
    [DbContext(typeof(PollContext))]
    [Migration("20180621112628_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Probel.Arbitrium.Models.Choice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("PollId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("Probel.Arbitrium.Models.Decision", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ChoiceId");

                    b.Property<DateTime>("Date");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ChoiceId");

                    b.HasIndex("UserId");

                    b.ToTable("Decisions");
                });

            modelBuilder.Entity("Probel.Arbitrium.Models.Poll", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Question");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("Probel.Arbitrium.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("PasswordHash");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Probel.Arbitrium.Models.Choice", b =>
                {
                    b.HasOne("Probel.Arbitrium.Models.Poll", "Poll")
                        .WithMany("Choices")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Probel.Arbitrium.Models.Decision", b =>
                {
                    b.HasOne("Probel.Arbitrium.Models.Choice", "Choice")
                        .WithMany("Decisions")
                        .HasForeignKey("ChoiceId");

                    b.HasOne("Probel.Arbitrium.Models.User", "User")
                        .WithMany("Decisions")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
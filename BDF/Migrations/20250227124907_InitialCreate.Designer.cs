﻿// <auto-generated />
using BDF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BDF.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250227124907_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Eleve", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EleveParrainId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FamilleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("MDP")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("BLOB");

                    b.Property<string>("Prenom")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PromotionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EleveParrainId");

                    b.HasIndex("FamilleId");

                    b.HasIndex("PromotionId");

                    b.ToTable("Eleves");
                });

            modelBuilder.Entity("Famille", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CouleurHexa")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Familles");
                });

            modelBuilder.Entity("Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Annee")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("Questionnaire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Astro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Boisson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Defaut")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EleveId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Film")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Livre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasseTemps")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Preference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Provenance")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Qualite")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Relation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Soiree")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Son")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EleveId");

                    b.ToTable("Questionnaires");
                });

            modelBuilder.Entity("Voeu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EleveChoisiId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EleveId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumVoeux")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PromotionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EleveChoisiId");

                    b.HasIndex("EleveId");

                    b.HasIndex("PromotionId");

                    b.ToTable("Voeux");
                });

            modelBuilder.Entity("Eleve", b =>
                {
                    b.HasOne("Eleve", "EleveParrain")
                        .WithMany()
                        .HasForeignKey("EleveParrainId");

                    b.HasOne("Famille", "Famille")
                        .WithMany()
                        .HasForeignKey("FamilleId");

                    b.HasOne("Promotion", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId");

                    b.Navigation("EleveParrain");

                    b.Navigation("Famille");

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("Questionnaire", b =>
                {
                    b.HasOne("Eleve", "Eleve")
                        .WithMany()
                        .HasForeignKey("EleveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Eleve");
                });

            modelBuilder.Entity("Voeu", b =>
                {
                    b.HasOne("Eleve", "EleveChoisi")
                        .WithMany()
                        .HasForeignKey("EleveChoisiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eleve", "Eleve")
                        .WithMany()
                        .HasForeignKey("EleveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Promotion", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Eleve");

                    b.Navigation("EleveChoisi");

                    b.Navigation("Promotion");
                });
#pragma warning restore 612, 618
        }
    }
}

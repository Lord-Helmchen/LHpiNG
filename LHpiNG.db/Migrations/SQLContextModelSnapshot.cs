﻿// <auto-generated />
using System;
using LHpiNG.db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LHpiNG.db.Migrations
{
    [DbContext(typeof(SQLContext))]
    partial class SQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("LHpi")
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LHpiNG.Album.AlbumObject", b =>
                {
                    b.Property<string>("OracleName");

                    b.Property<string>("Version");

                    b.Property<string>("SetTLA")
                        .HasColumnName("Set");

                    b.Property<int>("LanguageId")
                        .HasColumnName("Language");

                    b.Property<int?>("CollNr");

                    b.Property<string>("Color");

                    b.Property<int>("Foilage");

                    b.Property<string>("Number");

                    b.Property<int>("Rarity");

                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("OracleName", "Version", "SetTLA", "LanguageId");

                    b.HasAlternateKey("Uid");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SetTLA");

                    b.ToTable("AlbumObjects");
                });

            modelBuilder.Entity("LHpiNG.Album.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abbr");

                    b.Property<string>("M15Abbr");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("LHpiNG.Album.Set", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("CardCount");

                    b.Property<int>("Foilage");

                    b.Property<int?>("InsertCount");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("NontraditionalCount");

                    b.Property<int?>("ReplicaCount");

                    b.Property<string>("TLA")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<int?>("TokenCount");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.ExpansionEntity", b =>
                {
                    b.Property<string>("EnName")
                        .HasColumnName("Name");

                    b.Property<string>("Abbreviation");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("IdExpansion");

                    b.Property<bool?>("IsReleased");

                    b.Property<string>("ReleaseDate");

                    b.HasKey("EnName");

                    b.ToTable("Expansions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ExpansionEntity");
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.PriceGuideEntity", b =>
                {
                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("Avg");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<decimal?>("Low");

                    b.Property<decimal?>("LowFoil");

                    b.Property<decimal?>("LowexPlus");

                    b.Property<decimal?>("Sell");

                    b.Property<decimal?>("Trend");

                    b.HasKey("Uid");

                    b.ToTable("PriceGuides");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PriceGuideEntity");
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.ProductEntity", b =>
                {
                    b.Property<string>("EnName")
                        .HasColumnName("Name");

                    b.Property<string>("ExpansionName");

                    b.Property<int?>("CountReprints");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("IdMetaproduct");

                    b.Property<int?>("IdProduct");

                    b.Property<string>("Number");

                    b.Property<int>("Rarity");

                    b.Property<string>("Website");

                    b.HasKey("EnName", "ExpansionName");

                    b.HasIndex("ExpansionName");

                    b.ToTable("Products");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ProductEntity");
                });

            modelBuilder.Entity("LHpiNG.db.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpansionListFetchDate");

                    b.HasKey("Id");

                    b.ToTable("State");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ExpansionListFetchDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.Expansion", b =>
                {
                    b.HasBaseType("LHpiNG.Cardmarket.ExpansionEntity");

                    b.Property<int?>("ProductCount");

                    b.Property<string>("ProductsUrlSuffix");

                    b.Property<DateTime?>("ReleaseDateTime");

                    b.Property<string>("UrlSuffix");

                    b.ToTable("Expansions");

                    b.HasDiscriminator().HasValue("Expansion");
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.PriceGuide", b =>
                {
                    b.HasBaseType("LHpiNG.Cardmarket.PriceGuideEntity");

                    b.Property<string>("ExpansionName")
                        .IsRequired();

                    b.Property<DateTime>("FetchDate");

                    b.Property<decimal?>("FoilSell");

                    b.Property<decimal?>("FoilTrend");

                    b.Property<decimal?>("GermanProLow");

                    b.Property<int?>("IdProduct");

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.Property<decimal?>("Suggested");

                    b.HasIndex("ProductName", "ExpansionName");

                    b.ToTable("PriceGuides");

                    b.HasDiscriminator().HasValue("PriceGuide");
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.Product", b =>
                {
                    b.HasBaseType("LHpiNG.Cardmarket.ProductEntity");

                    b.Property<int?>("CollNr");

                    b.ToTable("Products");

                    b.HasDiscriminator().HasValue("Product");
                });

            modelBuilder.Entity("LHpiNG.Album.AlbumObject", b =>
                {
                    b.HasOne("LHpiNG.Album.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LHpiNG.Album.Set", "Set")
                        .WithMany("AlbumObjects")
                        .HasForeignKey("SetTLA")
                        .HasPrincipalKey("TLA")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.ProductEntity", b =>
                {
                    b.HasOne("LHpiNG.Cardmarket.ExpansionEntity", "Expansion")
                        .WithMany()
                        .HasForeignKey("ExpansionName")
                        .HasConstraintName("FK_Products_Expansions_ExpansionName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.PriceGuide", b =>
                {
                    b.HasOne("LHpiNG.Cardmarket.Product", "Product")
                        .WithMany("PriceGuides")
                        .HasForeignKey("ProductName", "ExpansionName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LHpiNG.Cardmarket.Product", b =>
                {
                    b.HasOne("LHpiNG.Cardmarket.Expansion")
                        .WithMany("Products")
                        .HasForeignKey("ExpansionName")
                        .HasConstraintName("FK_Products_Expansions_ExpansionName")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

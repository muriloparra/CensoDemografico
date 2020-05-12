﻿// <auto-generated />
using System;
using CensoDemografico.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CensoDemografico.Infra.Migrations
{
    [DbContext(typeof(CensoDemograficoContext))]
    partial class CensoDemograficoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CensoDemografico.Domain.Entities.Pessoa", b =>
                {
                    b.Property<int>("PessoaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Escolaridade");

                    b.Property<int>("Etnia");

                    b.Property<int>("Genero");

                    b.Property<int?>("IdMae");

                    b.Property<int?>("IdPai");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Regiao");

                    b.Property<string>("Sobrenome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("PessoaId");

                    b.HasIndex("IdMae");

                    b.HasIndex("IdPai");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("CensoDemografico.Domain.Entities.Pessoa", b =>
                {
                    b.HasOne("CensoDemografico.Domain.Entities.Pessoa", "Mae")
                        .WithMany()
                        .HasForeignKey("IdMae");

                    b.HasOne("CensoDemografico.Domain.Entities.Pessoa", "Pai")
                        .WithMany()
                        .HasForeignKey("IdPai");
                });
#pragma warning restore 612, 618
        }
    }
}

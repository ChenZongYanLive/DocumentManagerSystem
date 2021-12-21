using DocumentLibrary.MySql.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DocumentLibrary.MySql
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext()
        {

        }

        public DocumentDbContext(DbContextOptions<DocumentDbContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<AccountRule> Accountrule { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<DocumentHistory> Documenthistory { get; set; }
        public virtual DbSet<DocumentIndex> Documentindex { get; set; }
        public virtual DbSet<DocumentVersionHistory> Documentversionhistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRule>(entity =>
            {
                entity.ToTable("accountrule");

                entity.Property(e => e.Account).HasColumnType("varchar(200)");

                entity.Property(e => e.CreateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.IsDelete)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Rule).HasColumnType("varchar(45)");

                entity.Property(e => e.UpdateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("document");

                entity.Property(e => e.CreateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DocumentEndNumber).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.DocumentNumber).HasColumnType("varchar(250)");

                entity.Property(e => e.DocumentOwner).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentStartNumber).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentVersion).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentYearMonth).HasColumnType("varchar(45)");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.FileContent).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.FileType).HasColumnType("varchar(30)");

                entity.Property(e => e.IsDelete)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsInvalid)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsPublish)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                /*
                 entity.Property(e => e.IsPublishRecordDocument)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");
                */

                //entity.Property(e => e.Organization).HasColumnType("varchar(500)");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasColumnType("text");

                entity.Property(e => e.UpdateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DocumentHistory>(entity =>
            {
                entity.ToTable("documenthistory");

                entity.Property(e => e.CreateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.FileContent)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.FileType).HasColumnType("varchar(30)");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasColumnType("text");
            });

            modelBuilder.Entity<DocumentIndex>(entity =>
            {
                entity.ToTable("documentindex");

                entity.Property(e => e.CreateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DocumentId).HasDefaultValueSql("'-1'");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.DocumentNumber)
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.DocumentOwner).HasColumnType("varchar(45)");

                entity.Property(e => e.IsDelete)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsInvalid)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.UpdateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DocumentVersionHistory>(entity =>
            {
                entity.ToTable("documentversionhistory");

                entity.Property(e => e.CreateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DocumentEndNumber).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.DocumentNumber).HasColumnType("varchar(250)");

                entity.Property(e => e.DocumentOwner).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentStartNumber).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentVersion).HasColumnType("varchar(45)");

                entity.Property(e => e.DocumentYearMonth).HasColumnType("varchar(45)");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.FileContent).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.FileType).HasColumnType("varchar(30)");

                entity.Property(e => e.IsDelete)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsInvalid)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsPublish)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasColumnType("text");

                entity.Property(e => e.UpdateBy).HasColumnType("varchar(45)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });
        }
    }
}

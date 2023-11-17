using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MMABooksEFClasses.models
{
    public partial class MMABooksContext : DbContext
    {
        public MMABooksContext()
        {
        }

        public MMABooksContext(DbContextOptions<MMABooksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<Invoicelineitem> Invoicelineitems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;uid=root;pwd=Ih2cnFaG;database=mmabooksframework", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PRIMARY");

                entity.ToTable("customers");

                entity.HasIndex(e => e.StateCode)
					.HasName("FK_Customers_States");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StateCode).HasColumnName("State")
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.StateCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_States");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("invoices");

                entity.HasIndex(e => e.CustomerId, "FK_Invoices_Customers");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceTotal).HasPrecision(10, 4);

                entity.Property(e => e.ProductTotal).HasPrecision(10, 4);

                entity.Property(e => e.SalesTax).HasPrecision(10, 4);

                entity.Property(e => e.Shipping).HasPrecision(10, 4);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Invoices_Customers");
            });

            modelBuilder.Entity<Invoicelineitem>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceId, e.ProductCode })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("invoicelineitems");

                entity.HasIndex(e => e.ProductCode, "FK_InvoiceLineItems_Products");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ItemTotal).HasPrecision(10, 4);

                entity.Property(e => e.UnitPrice).HasPrecision(10, 4);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Invoicelineitems)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_InvoiceLineItems_Invoices");

                entity.HasOne(d => d.ProductCodeNavigation)
                    .WithMany(p => p.Invoicelineitems)
                    .HasForeignKey(d => d.ProductCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceLineItems_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductCode)
                    .HasName("PRIMARY");

                entity.ToTable("products");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasPrecision(10, 4);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.StateCode)
                    .HasName("PRIMARY");

                entity.ToTable("states");

                entity.Property(e => e.StateCode)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.StateName).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

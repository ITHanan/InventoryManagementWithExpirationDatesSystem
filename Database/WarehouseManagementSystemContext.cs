using System;
using System.Collections.Generic;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementWithExpirationDatesSystem.Database;

public partial class WarehouseManagementSystemContext : DbContext
{
    public WarehouseManagementSystemContext()
    {
    }

    public WarehouseManagementSystemContext(DbContextOptions<WarehouseManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HANAN\\SQLEXPRESS;Database=WarehouseManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E83EB18948CD4");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stock__2C83A9E201721A72");

            entity.ToTable("Stock");

            entity.Property(e => e.StockId).HasColumnName("StockID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ReceivedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Item).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__ItemID__3C69FB99");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__SupplierI__3D5E1FD2");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666944D20467C");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.ContactPerson).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

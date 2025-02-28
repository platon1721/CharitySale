using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductType> ProductTypes { get; set; } = default!;
    public DbSet<Receipt> Receipts { get; set; } = default!;
    public DbSet<ReceiptProduct> ReceiptProducts { get; set; } = default!;
    
    public DbSet<MoneyTransaction> MoneyTransactions { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure composite key for ReceiptProduct
        modelBuilder.Entity<ReceiptProduct>()
            .HasKey(rp => new { rp.ReceiptId, rp.ProductId });

        // Configure relationships
        modelBuilder.Entity<ReceiptProduct>()
            .HasOne(rp => rp.Receipt)
            .WithMany(r => r.ReceiptProducts)
            .HasForeignKey(rp => rp.ReceiptId);

        modelBuilder.Entity<ReceiptProduct>()
            .HasOne(rp => rp.Product)
            .WithMany(p => p.ReceiptProducts)
            .HasForeignKey(rp => rp.ProductId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductType)
            .WithMany(pt => pt.Products)
            .HasForeignKey(p => p.ProductTypeId);

        modelBuilder.Entity<Receipt>()
            .HasOne(r => r.User)
            .WithMany(u => u.Receipts)
            .HasForeignKey(r => r.UserId);
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Receipt>()
            .Property(r => r.PaidAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ReceiptProduct>()
            .Property(rp => rp.UnitPrice)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }
}
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Intex.Models;

public partial class IntexContext : DbContext
{
    public IntexContext()
    {
    }

    public IntexContext(DbContextOptions<IntexContext> options)
        : base(options)
    {
    }

    public IntexContext(string connectionString) : base(GetOptions(connectionString))
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Orders> Orderss { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ItemRecommendation> ItemRecommendations { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("");

    private static DbContextOptions<IntexContext> GetOptions(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IntexContext>();
        optionsBuilder.UseSqlServer(connectionString); // Adjust this based on your database provider
        return optionsBuilder.Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("customer");

            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence).HasColumnName("country_of_residence");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.LastName).HasColumnName("last_name");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("lineItem");

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("orders");

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.CountryOfTransaction).HasColumnName("country_of_transaction");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.EntryMode).HasColumnName("entry_mode");
            entity.Property(e => e.Fraud).HasColumnName("fraud");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
            entity.Property(e => e.TypeOfCard).HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("type_of_transaction");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("product");

            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImgLink).HasColumnName("img_link");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor).HasColumnName("primary_color");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<ItemRecommendation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("itemRecs");

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.ProductName).HasColumnName("productName");
            entity.Property(e => e.Rec1Id).HasColumnName("rec1_ID");
            entity.Property(e => e.Rec1Name).HasColumnName("rec1_Name");
            entity.Property(e => e.Rec2Id).HasColumnName("rec2_ID");
            entity.Property(e => e.Rec2Name).HasColumnName("rec2_Name");
            entity.Property(e => e.Rec3Id).HasColumnName("rec3_ID");
            entity.Property(e => e.Rec3Name).HasColumnName("rec3_Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

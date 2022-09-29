﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopingList.Data.Models;

namespace ShopingList.Data;

public class ShopingListDBContext : IdentityDbContext<IdentityUser>
{
    public ShopingListDBContext(DbContextOptions<ShopingListDBContext> options)
        : base(options)
    {
    }

    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Models.ShopingList> ShopingLists { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().HasOne(p => p.Category);

        builder.Entity<ProductCategory>()
            .HasData(
                new { Id = 1, Name = "Fruits" },
                new { Id = 2, Name = "Vegetables" },
                new { Id = 3, Name = "Frozen Foods" },
                new { Id = 4, Name = "Meat" },
                new { Id = 5, Name = "Beverages" }
            );

        builder.Entity<Product>()
            .HasData(
                new { Id = 1, Name = "Orange", CategoryId = 1 },
                new { Id = 2, Name = "Apple", CategoryId = 1 },
                new { Id = 3, Name = "Cuccumber", CategoryId = 2 },
                new { Id = 4, Name = "Domato", CategoryId = 2 },
                new { Id = 5, Name = "Ice cream", CategoryId = 3 },
                new { Id = 6, Name = "Pizza", CategoryId = 3 },
                new { Id = 7, Name = "Beef", CategoryId = 4 },
                new { Id = 8, Name = "Pork", CategoryId = 4 },
                new { Id = 9, Name = "Soda", CategoryId = 5 },
                new { Id = 10, Name = "Beer", CategoryId = 5 }
            );

        //builder.Entity<Models.ShopingList>()
        //    .HasMany(sp => sp.Products)
        //    .WithMany(p => p.ShopingLists);
            //.Map(sp =>
            // {
            //     sp.MapLeftKey("ShopingListRefId");
            //     sp.MapRightKey("ProductRefId");
            //     sp.ToTable("ShopingListProduct");
            // });

        base.OnModelCreating(builder);
    }
}
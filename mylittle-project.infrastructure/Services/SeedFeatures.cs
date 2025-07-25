using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace mylittle_project.infrastructure.Services
{
    public static class SeedFeatures
    {
        public static async Task RunAsync(AppDbContext ctx)
        {
            // If any FeatureModules exist, skip seeding
            if (await ctx.FeatureModules.AnyAsync())
                return;

            // 1) Catalog Management Module
            var catalogModule = new FeatureModule
            {
                Id = Guid.NewGuid(),
                Key = "catalog",
                Name = "Catalog Management",
                Description = "Product and inventory management features",
                Features = new List<Feature>
                {
                    new Feature { Id = Guid.NewGuid(), Key = "products", Name = "Products" },
                    new Feature { Id = Guid.NewGuid(), Key = "brands", Name = "Brands" },
                    new Feature { Id = Guid.NewGuid(), Key = "reviews", Name = "Reviews" },
                    new Feature { Id = Guid.NewGuid(), Key = "product-tags", Name = "Product Tags", IsPremium = true },
                    new Feature { Id = Guid.NewGuid(), Key = "categories", Name = "Categories", Description = "Manage product categories and filters", IsPremium = false },
                    new Feature { Id = Guid.NewGuid(), Key = "filters", Name = "Filters", Description = "Manage filters assigned to product categories", IsPremium = false },
                    new Feature { Id = Guid.NewGuid(), Key = "product-fields", Name = "Product Fields", Description = "Custom fields for products", IsPremium = true }
                }
            };

            // 2) Sales & Orders Module
            var salesModule = new FeatureModule
            {
                Id = Guid.NewGuid(),
                Key = "sales",
                Name = "Sales & Orders",
                Description = "Order processing and sales management",
                Features = new List<Feature>
                {
                    new Feature { Id = Guid.NewGuid(), Key = "orders", Name = "Orders" },
                    new Feature { Id = Guid.NewGuid(), Key = "discounts", Name = "Discounts & Offers", IsPremium = true }
                }
            };

            // Add all modules to the context and save
            ctx.FeatureModules.AddRange(catalogModule, salesModule);
            await ctx.SaveChangesAsync();
        }
    }
}

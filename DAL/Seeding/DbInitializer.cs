using DAL.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL.Seeding
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                
                // Veendume, et andmebaas on loodud (ja kasutame migratsioone, kui need on olemas)
                context.Database.Migrate();
                
                // Külvame andmed
                SeedData(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<AppDbContext>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private static void SeedData(AppDbContext context)
        {
            // Check if DB has already data
            if (context.Users.Any() && context.ProductTypes.Any() && context.Products.Any())
            {
                return;
            }

            // Add default product types
            if (!context.ProductTypes.Any())
            {
                var productTypes = new List<ProductType>
                {
                    new() { Name = "Food", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow },
                    new() { Name = "Drinks", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow },
                    new() { Name = "Clothing", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow },
                };
                
                context.ProductTypes.AddRange(productTypes);
                context.SaveChanges();
            }

            // Add default users
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new() 
                    { 
                        Firstname = "Admin", 
                        Surname = "User", 
                        Login = "admin", 
                        IsAdmin = true,
                        CreatedAt = DateTime.UtcNow, 
                        ModifiedAt = DateTime.UtcNow 
                    },
                    new() 
                    { 
                        Firstname = "Regular", 
                        Surname = "User", 
                        Login = "user", 
                        IsAdmin = false,
                        CreatedAt = DateTime.UtcNow, 
                        ModifiedAt = DateTime.UtcNow 
                    }
                };
                
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // Add default products
            if (!context.Products.Any())
            {
                var foodTypeId = context.ProductTypes.FirstOrDefault(pt => pt.Name == "Food")?.Id ?? 1;
                var drinksTypeId = context.ProductTypes.FirstOrDefault(pt => pt.Name == "Drinks")?.Id ?? 2;
                var clothingTypeId = context.ProductTypes.FirstOrDefault(pt => pt.Name == "Clothing")?.Id ?? 3;

                var products = new List<Product>
                {
                    new() 
                    { 
                        Name = "Apple Pie", 
                        Description = "Fresh homemade apple pie",
                        Price = 3.00m,
                        Stock = 10,
                        ProductTypeId = foodTypeId,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    },
                    new() 
                    { 
                        Name = "Coffee", 
                        Description = "Hot aromatic coffee",
                        Price = 2.50m,
                        Stock = 50,
                        ProductTypeId = drinksTypeId,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    },
                    new() 
                    { 
                        Name = "T-shirt", 
                        Description = "Cotton t-shirt",
                        Price = 5.00m,
                        Stock = 20,
                        ProductTypeId = clothingTypeId,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    }
                };
                
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
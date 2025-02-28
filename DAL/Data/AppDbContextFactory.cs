using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "Host=localhost;Database=bakesale;Username=postgres;Password=postgres"
            : "Host=db;Database=bakesale;Username=postgres;Password=postgres";
            
        optionsBuilder.UseNpgsql(connectionString
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
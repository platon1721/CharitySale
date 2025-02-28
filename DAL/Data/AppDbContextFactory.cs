using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Use config file
        IConfigurationRoot configuration = null;
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        try
        {
            // Try to find appsettings.json
            string projectDir = Directory.GetCurrentDirectory();
            
            while (projectDir != null && !File.Exists(Path.Combine(projectDir, "appsettings.json")))
            {
                projectDir = Directory.GetParent(projectDir)?.FullName;
            }
            
            if (projectDir != null)
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();
                
                var connString = configuration.GetConnectionString("LocalConnection");
                if (!string.IsNullOrEmpty(connString))
                {
                    optionsBuilder.UseNpgsql(connString);
                    return new AppDbContext(optionsBuilder.Options);
                }
            }
        }
        catch
        {
            
        }
        
        string envConnString = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrEmpty(envConnString))
        {
            optionsBuilder.UseNpgsql(envConnString);
            return new AppDbContext(optionsBuilder.Options);
        }
        
        // If nothing work, uses default connection string.
        optionsBuilder.UseNpgsql("Host=localhost;Database=bakesale;Username=postgres;Password=postgres");
        return new AppDbContext(optionsBuilder.Options);
    }
}
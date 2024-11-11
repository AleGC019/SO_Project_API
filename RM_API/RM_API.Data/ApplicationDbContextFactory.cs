using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RM_API.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build the configuration by explicitly setting the path to RM_API.API directory
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../RM_API.API");
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)  // Explicitly set the base path to RM_API.API directory
                .AddJsonFile("appsettings.json")  // Use appsettings.json in RM_API.API
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
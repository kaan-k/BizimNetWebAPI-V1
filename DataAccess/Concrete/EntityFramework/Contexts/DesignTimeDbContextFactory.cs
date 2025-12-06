using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BizimNetWebAPI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BizimNetContext>
    {
        public BizimNetContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BizimNetContext>();

            // 1. Choose your provider (PostgreSQL or SQL Server)
            // Ensure you have the NuGet package installed: Npgsql.EntityFrameworkCore.PostgreSQL
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=BizimNetDB;User Id=postgres;Password=yourpassword;");

            return new BizimNetContext(optionsBuilder.Options);
        }
    }
}
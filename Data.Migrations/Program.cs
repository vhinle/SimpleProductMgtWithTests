using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options =>
                options.UseMySql("Server=localhost;Database=productsdb;User ID=root;Password=;",
                    new MySqlServerVersion(new Version(8, 0, 21))))
            .BuildServiceProvider();

            using (var context = serviceProvider.GetRequiredService<AppDbContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}
using ECommerce.Application.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<ECommerce.Application.Core.Entities.Product> Product { get; set; }
        public DbSet<ECommerce.Application.Core.Entities.Order> Order { get; set; }

    }
}

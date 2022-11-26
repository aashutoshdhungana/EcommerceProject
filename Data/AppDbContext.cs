using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EcommerceMVC.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<ECommerce.Application.Core.Entities.Product> Product { get; set; }

    }
}

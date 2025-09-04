using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorRbacDemo.Models;

namespace BlazorRbacDemo;

public class AppUser : IdentityUser { }

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Optional: ensure decimal precision for non-SQLite providers
        builder.Entity<Order>()
               .Property(o => o.Amount)
               .HasColumnType("decimal(18,2)");
    }
}

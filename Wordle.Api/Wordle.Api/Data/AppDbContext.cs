using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Metadata;

namespace Wordle.Api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Leader> Leaders => Set<Leader>();
        public DbSet<Civ> Civs => Set<Civ>();
        public DbSet<LeaderAttribute> LeaderAttributes => Set<LeaderAttribute>();
        public DbSet<CivAttribute> CivAttributes => Set<CivAttribute>();
        public DbSet<CivBackground> CivBackgrounds => Set<CivBackground>();

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<DateWord>()
                .HasIndex(f => f.Date)
                .IsUnique();
           
        }*/
    }
}

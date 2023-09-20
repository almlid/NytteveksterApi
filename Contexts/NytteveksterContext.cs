using Microsoft.EntityFrameworkCore;
using NytteveksterApi.Models;
using Type = NytteveksterApi.Models.Type;

namespace NytteveksterApi.Contexts;

public class NytteveksterContext : DbContext
{
  public NytteveksterContext(DbContextOptions<NytteveksterContext> options) : base(options) { }

  public DbSet<Type> Types { get; set; }
  public DbSet<Species> Species { get; set; }
  public DbSet<SpeciesAvailability> SpeciesAvailability { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Type>().HasKey(t => t.Id);

    modelBuilder.Entity<Species>().HasKey(s => s.Id);
    modelBuilder.Entity<Species>().HasOne(s => s.Type).WithMany(t => t.Species);

    modelBuilder.Entity<SpeciesAvailability>().HasKey(sa => sa.Id);
    modelBuilder.Entity<SpeciesAvailability>()
        .HasOne(sa => sa.Species)
        .WithOne(s => s.Availability)
        .HasForeignKey<SpeciesAvailability>(sa => sa.Id);
  }
}

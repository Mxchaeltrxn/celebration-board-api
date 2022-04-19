using System.Reflection;
using CelebrationBoard.Domain.Celebrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class CelebrationBoardContext : DbContext
{
  public DbSet<Celebration> Celebration { get; set; }
  public DbSet<CelebrationTag> CelebrationTag { get; set; }
  private readonly bool canLogToConsole;
  private readonly string connectionString;
  public CelebrationBoardContext(string connectionString, bool canLogToConsole)
  {
    this.connectionString = connectionString;
    this.canLogToConsole = canLogToConsole;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql(connectionString);

    if (canLogToConsole)
    {
      optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
    }
  }

  protected override void OnModelCreating(ModelBuilder modelbuilder)
  {
    base.OnModelCreating(modelbuilder);
    modelbuilder.Entity<Celebration>(x =>
        {
          x.ToTable("Celebration")
            .HasKey(k => k.Id);
          x.Navigation(e => e.Tags).AutoInclude();
          x.Property(p => p.Title)
    .HasConversion(
      title => title.Value,
      dbValue => (Title)dbValue);
          x.Property(p => p.Content)
    .HasConversion(
      content => content.Value,
      dbValue => (Content)dbValue);
          x.Property(p => p.AccessLevel)
    .HasConversion(
      accessLevel => accessLevel.ToString(),
      dbValue => (PrivacyLevel)Enum.Parse(typeof(PrivacyLevel), dbValue));
        });

    modelbuilder.Entity<CelebrationTag>(x =>
    {
      x.ToTable("CelebrationTag")
        .HasKey(k => k.Id);
      x.Property(p => p.Name)
  .HasConversion(
    tagName => tagName.Value,
    dbValue => (TagName)dbValue);
      // x.Navigation(e => e.Celebrations).AutoInclude();
    });

    // modelbuilder.Entity<CelebrationAndTagRelationship>(x =>
    // {
    //   // x.ToTable("CelebrationAndTagRelationship")
    //   // .HasKey(k => new { k.CelebrationId, k.CelebrationTagId });
    //   x
    //     .HasKey(t => new { t.CelebrationId, t.TagId });

    //   x
    //   .HasOne(pt => pt.Celebration)
    //   .WithMany(p => p.CelebrationAndTagRelationship)
    //   .HasForeignKey(pt => pt.CelebrationId);

    //   x
    //   .HasOne(pt => pt.Tag)
    //   .WithMany(t => t.CelebrationAndTagRelationship)
    //   .HasForeignKey(pt => pt.TagId);
    // });

    // v => v.ToString(),
    //             v => (EquineBeast)Enum.Parse(typeof(EquineBeast), v))

    modelbuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}

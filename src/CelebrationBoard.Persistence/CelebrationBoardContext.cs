using System.Reflection;
using CelebrationBoard.Domain.Celebrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class CelebrationBoardContext : DbContext
{
  public DbSet<Celebration> Celebration { get; set; }
  public DbSet<Tag> CelebrationTag { get; set; }
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

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    // tosave: https://stackoverflow.com/questions/69252014/entity-framework-core-multiple-many-to-many-with-linking-table-join-entity
    // tosave: https://stackoverflow.com/questions/54196199/entity-framework-core-multiple-one-to-many-relationships-between-two-entities
    // modelBuilder.Entity<FavouritedUserCelebration>().HasKey(e => new { e.CelebrationId, e.UserId });

    modelBuilder.Entity<User>(x =>
    {
      x.ToTable("User").HasKey(y => y.Id);
      x.HasMany(y => y.Celebrations).WithOne(y => y.User);

    });
    modelBuilder.Entity<Celebration>(x =>
        {
          x.ToTable("Celebration")
            .HasKey(k => k.Id);
          x.Navigation(e => e.Tags).AutoInclude();
          x.HasMany(e => e.FavouritedUsers)
          .WithMany(e => e.FavouritedCelebrations)
          .UsingEntity<Dictionary<string, object>>(
            "UserFavouritedCelebration",
            x => x.HasOne<User>().WithMany(),
            x => x.HasOne<Celebration>().WithMany());
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
      accessLevel => accessLevel.Value,
      dbValue => (PrivacyLevel)dbValue!);
        });

    modelBuilder.Entity<Tag>(x =>
    {
      x.ToTable("Tag")
        .HasKey(k => k.Id);
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

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}

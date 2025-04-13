// Ce fichier configure la base de données avec Entity Framework Core.
// Il déclare les DbSet pour chaque entité (Eleve, Famille, Promotion, Questionnaire, Voeu).
// Il utilise SQLite comme base de données locale, avec un comportement optimisé pour la lecture.

using Microsoft.EntityFrameworkCore;

namespace BDF.Data;

public class DataContext : DbContext
{
  public DbSet<Eleve> Eleves { get; set; } = null!;
  public DbSet<Famille> Familles { get; set; } = null!;
  public DbSet<Promotion> Promotions { get; set; } = null!;
  public DbSet<Questionnaire> Questionnaires { get; set; } = null!;
  public DbSet<Voeu> Voeux { get; set; } = null!;
  public string DbPath { get; private set; }
    public object GestionClients { get; internal set; }

    public DataContext()
  {
    // Path to SQLite database file
    DbPath = "BDF.db";
  }


  // The following configures EF to create a SQLite database file locally
  protected override void OnConfiguring(DbContextOptionsBuilder options)
  {
    // Use SQLite as database
    options.UseSqlite($"Data Source={DbPath}")
                  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // Évite les verrous en lecture
                  .EnableSensitiveDataLogging();
    // Optional: log SQL queries to console
    //options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
  }
}
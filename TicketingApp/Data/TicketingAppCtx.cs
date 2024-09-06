using Microsoft.EntityFrameworkCore;
using TicketingApp.Models;

namespace TicketingApp.Data;

public class TicketingAppCtx(IConfiguration config, DbContextOptions options) : DbContext(options)
{
    public static readonly string TicketingAppSchema = "ticketing_app_schema";
    public static readonly string MigrationsTable = "_migrations";
    protected readonly string connectionString = config.GetConnectionString("postgres") ?? throw new Exception("Connection string not found");

    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ticketing_app_schema");

        modelBuilder.Entity<Artist>()
            .HasGeneratedTsVectorColumn(a =>
                a.SearchVector!,
                "english",
                a => new { a.Name }
            )
            .HasIndex(a => a.SearchVector)
            .HasMethod("GIN");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString, x => x.MigrationsHistoryTable(MigrationsTable, TicketingAppSchema));
    }
}
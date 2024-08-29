using Microsoft.EntityFrameworkCore;
using TicketingApp.Models;

namespace TicketingApp.Data;

public class TicketingAppCtx(DbContextOptions options) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>()
            .HasGeneratedTsVectorColumn(a =>
                a.SearchVector!,
                "english",
                a => new { a.Name }
            )
            .HasIndex(a => a.SearchVector)
            .HasMethod("GIN");
    }
}
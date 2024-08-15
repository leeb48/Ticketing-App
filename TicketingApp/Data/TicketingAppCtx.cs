using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TicketingApp.Models;

namespace TicketingApp.Data;

public class TicketCtx(DbContextOptions options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();
}
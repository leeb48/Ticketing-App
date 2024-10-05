using System.Reflection.Metadata.Ecma335;
using StackExchange.Redis;
using TicketingApp.Models;

namespace TicketingApp.Services.TicketLockService;

public class TicketLockService : ILockService<Ticket>
{
    readonly IDatabase redis;
    public TicketLockService()
    {
        redis = ConnectionMultiplexer.Connect("localhost").GetDatabase();
    }

    public async void CreateLock(List<Ticket> tickets, int TTL)
    {
        foreach (var ticket in tickets)
        {
            var ticketId = ticket.Id.ToString();
            var ticketLock = await redis.StringGetAsync(ticketId);

            if (!ticketLock.IsNullOrEmpty)
            {
                continue;
            }

            await redis.StringSetAsync(ticketId, ticketId);
            await redis.StringGetSetExpiryAsync(ticketId, TimeSpan.FromSeconds(TTL));
        }
    }

    public async Task<List<Ticket>> GetLockedEntities(List<Ticket> tickets)
    {
        var lockedTickets = new List<Ticket>();

        foreach (var ticket in tickets)
        {
            var ticketId = ticket.Id.ToString();
            var ticketLock = await redis.StringGetAsync(ticketId);

            if (!ticketLock.IsNullOrEmpty)
            {
                lockedTickets.Add(ticket);
            }
        }

        return lockedTickets;
    }
}
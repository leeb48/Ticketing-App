namespace TicketingApp.Models;

public class EventDetailView
{
    public required Event Event { get; set; }
    public required List<Seat> ReservedSeats { get; set; }
}
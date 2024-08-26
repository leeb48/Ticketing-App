namespace TicketingApp.Models;

public class User
{
    public int Id { get; set; }

    public List<Booking> Bookings { get; set; } = null!;
}
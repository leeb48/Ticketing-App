using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Venue
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    public List<Event> Event { get; set; } = null!;

    public List<Seat> Seats { get; set; } = null!;
}
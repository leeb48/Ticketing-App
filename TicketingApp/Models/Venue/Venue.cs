using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Venue : FullTextSearchEntity
{
    public int Id { get; set; }

    [Required]
    public string Address { get; set; } = string.Empty;

    public List<Event> Event { get; set; } = null!;

    public List<Seat> Seats { get; set; } = null!;
}
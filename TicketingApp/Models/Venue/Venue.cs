using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Venue : FullTextSearchEntity
{
    public int Id { get; set; }

    [Required]
    public string Address { get; set; } = string.Empty;

    public List<Event> Event { get; set; } = null!;

    [Required]
    public required int RowCount { get; set; }

    [Required]
    public required int ColumnCount { get; set; }

    public List<Seat> Seats { get; set; } = null!;
}
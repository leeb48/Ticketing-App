namespace TicketingApp.Models;

public class EventCreateDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string? Category { get; set; } = string.Empty;

    public int ArtistId { get; set; }

    public int VenueId { get; set; }
}
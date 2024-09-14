namespace TicketingApp.Models;

public class VenueCreateDto
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int RowCountInput { get; set; }
    public int ColCountInput { get; set; }
}
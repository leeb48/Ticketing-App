
using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class VenueCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    public required string SeatingChart { get; set; }
}
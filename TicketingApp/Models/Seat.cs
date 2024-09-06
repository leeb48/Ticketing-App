using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Seat
{
    public int Id { get; set; }

    [Required]
    public Venue Venue = null!;

    [Required]
    public string Row { get; set; } = string.Empty;

    [Required]
    public string Column { get; set; } = string.Empty;
}
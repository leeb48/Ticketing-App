using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Seat
{
    public int Id { get; set; }

    [Required]
    public Venue venue = null!;

    [Required]
    public string Section { get; set; } = string.Empty;

    [Required]
    public string Row { get; set; } = string.Empty;

    [Required]
    public string Column { get; set; } = string.Empty;
}
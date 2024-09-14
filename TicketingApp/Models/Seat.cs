using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Seat
{
    public int Id { get; set; }

    [Required]
    public Venue Venue = null!;

    [Required]
    public int Row { get; set; }

    [Required]
    public int Column { get; set; }
}
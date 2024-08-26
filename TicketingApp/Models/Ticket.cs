using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Ticket
{
    public int Id { get; set; }

    [Required]
    public Event Event { get; set; } = null!;

    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public Seat Seat { get; set; } = null!;
}
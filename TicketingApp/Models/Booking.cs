
using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public User User { get; set; } = null!;

    public List<Ticket> Tickets { get; set; } = null!;

    [Required]
    public decimal TotalCost { get; set; }

    [Required]
    public string status { get; set; } = null!;
}
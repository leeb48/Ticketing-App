using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Event
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    public string? Category { get; set; } = string.Empty;

    [Required]
    public Artist Artist { get; set; } = null!;

    [Required]
    public Venue Venue { get; set; } = null!;
}
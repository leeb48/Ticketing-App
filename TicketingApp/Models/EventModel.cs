using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Event
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
}
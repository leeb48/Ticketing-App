using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models;

public class Artist : FullTextSearchEntity
{
    public int Id { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public List<Event>? Event { get; set; }

}
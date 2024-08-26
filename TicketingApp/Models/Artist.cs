using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace TicketingApp.Models;

public class Artist
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public List<Event>? Event { get; set; }

    public NpgsqlTsVector? SearchVector { get; set; }
}
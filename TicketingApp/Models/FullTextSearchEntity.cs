using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace TicketingApp.Models;

public class FullTextSearchEntity
{
    [Required]
    public required string Name { get; set; } = string.Empty;

    public NpgsqlTsVector SearchVector { get; set; } = null!;
}
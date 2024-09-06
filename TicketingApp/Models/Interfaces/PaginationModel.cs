using System.ComponentModel.DataAnnotations;

namespace TicketingApp.Models.Interfaces;

public abstract class PaginationModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
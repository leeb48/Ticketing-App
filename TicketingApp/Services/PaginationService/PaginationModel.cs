namespace TicketingApp.Services.PaginationService;

public class PaginationModel<TEntity>
{
    public string SearchInput { get; set; } = string.Empty;
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public bool IsAdmin { get; set; } = false;
    public required List<TEntity> Entities { get; set; }
}
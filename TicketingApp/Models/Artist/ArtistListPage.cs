namespace TicketingApp.Models;

public class ArtistListPage
{
    public string SearchInput { get; set; } = string.Empty;
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public required List<Artist> Artists { get; set; }
}
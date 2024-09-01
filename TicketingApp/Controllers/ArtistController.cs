using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;

namespace TicketingApp.Controllers;

public class ArtistController(TicketingAppCtx ctx) : Controller
{
    private readonly TicketingAppCtx ctx = ctx;

    public async Task<IActionResult> Index()
    {
        var artistListPage = await ListAllArtists(5);

        return View(artistListPage);
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5)
    {
        if (string.IsNullOrEmpty(searchInput))
        {
            return PartialView("ArtistList", await ListAllArtists(pageSize));
        }

        var searchQuery = ctx.Artists
            .OrderBy(a => a.Name)
            .Where(a => a.SearchVector!.Matches(EF.Functions.ToTsQuery($"{searchInput}:*")));

        var artistListPage = new ArtistListPage
        {
            Artists = await searchQuery.Take(pageSize).ToListAsync(),
            PageSize = pageSize,
            PageCount = searchQuery.Count() / pageSize,
            SearchInput = searchInput,
        };

        return PartialView("ArtistList", artistListPage);
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage)
    {
        var artists = new List<Artist>();
        var artistListPage = new ArtistListPage { Artists = artists, CurrentPage = currentPage };

        if (string.IsNullOrEmpty(searchInput))
        {
            var getAllArtistQuery = ctx.Artists;

            artistListPage.PageCount = pageCount;
            artistListPage.PageSize = pageSize;
            artistListPage.Artists = await getAllArtistQuery
                .OrderBy(a => a.Name)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();


            return PartialView("ArtistList", artistListPage);
        }

        var searchQuery = ctx.Artists
            .OrderBy(a => a.Name)
            .Where(a => a.SearchVector!.Matches(EF.Functions.ToTsQuery($"{searchInput}:*")));

        artistListPage.PageCount = pageCount;
        artistListPage.PageSize = pageSize;
        artistListPage.Artists = await searchQuery.Skip(offset).Take(pageSize).ToListAsync();
        artistListPage.SearchInput = searchInput;

        return PartialView("ArtistList", artistListPage);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Artist artist)
    {
        if (!ModelState.IsValid)
        {
            return View(artist);
        }

        ctx.Add(artist);
        await ctx.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var artist = await ctx.Artists.FirstOrDefaultAsync(a => a.Id == id);
        if (artist == null)
        {
            return View("Error", new ErrorViewModel { Message = $"Artist with ID: {id} was not found" });
        }

        return View(artist);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Artist artist)
    {
        if (!ModelState.IsValid)
        {
            return View(artist);
        }

        ctx.Artists.Update(artist);
        var res = await ctx.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<ArtistListPage> ListAllArtists(int pageSize)
    {
        var artists = new List<Artist>();
        var artistListPage = new ArtistListPage { Artists = artists, PageSize = pageSize };

        var getAllArtistQuery = ctx.Artists;

        artistListPage.PageCount = getAllArtistQuery.Count() / pageSize;
        artistListPage.Artists = await getAllArtistQuery
            .OrderBy(a => a.Name)
            .Take(pageSize)
            .ToListAsync();


        return artistListPage;
    }
}
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
        var artists = await ctx.Artists
            .Take(10)
            .ToListAsync();

        var artistListPage = new ArtistListPage { Artists = artists };

        return View(artistListPage);
    }

    public IActionResult Create()
    {
        return View();
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

    [HttpPost]
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5)
    {
        var artists = new List<Artist>();
        var artistListPage = new ArtistListPage { Artists = artists, PageSize = pageSize };


        if (string.IsNullOrEmpty(searchInput))
        {
            var getAllArtistQuery = ctx.Artists;

            artistListPage.PageCount = getAllArtistQuery.Count() / pageSize;
            artistListPage.Artists = await getAllArtistQuery
                .OrderBy(a => a.Name)
                .Take(pageSize)
                .ToListAsync();


            return PartialView("ArtistList", artistListPage);
        }

        var searchQuery = ctx.Artists
            .OrderBy(a => a.Name)
            .Where(a => a.SearchVector!.Matches(EF.Functions.ToTsQuery($"{searchInput}:*")));

        artistListPage.PageCount = searchQuery.Count() / pageSize;
        artistListPage.Artists = await searchQuery.Take(pageSize).ToListAsync();
        artistListPage.SearchInput = searchInput;

        return PartialView("ArtistList", artistListPage);
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize)
    {
        var artists = new List<Artist>();
        var artistListPage = new ArtistListPage { Artists = artists };

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
}
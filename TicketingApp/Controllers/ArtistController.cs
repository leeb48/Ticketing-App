using System.Text.Json;
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

        return View(artists);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(string artistSearchInput)
    {
        var artists = new List<Artist>();

        if (string.IsNullOrEmpty(artistSearchInput))
        {
            return PartialView("ArtistList", artists);
        }

        artists = await ctx.Artists
            .Where(a => a.SearchVector!.Matches(EF.Functions.ToTsQuery($"{artistSearchInput}:*")))
            .Take(10)
            .ToListAsync();

        return PartialView("ArtistList", artists);
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
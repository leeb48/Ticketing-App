using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.PaginationService;

namespace TicketingApp.Controllers;

public class ArtistController(TicketingAppCtx ctx) : Controller
{
    private readonly PaginationService<Artist> paginationService = new(ctx);
    public async Task<IActionResult> Index()
    {
        return View(await paginationService.ListAllPage(5));
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5)
    {
        if (string.IsNullOrEmpty(searchInput))
        {
            return PartialView("ArtistList", await paginationService.ListAllPage(pageSize));
        }

        return PartialView("ArtistList", await paginationService.Search(searchInput, pageSize));
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage)
    {
        return PartialView("ArtistList", await paginationService.Pagination(searchInput, offset, pageCount, pageSize, currentPage));
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
}
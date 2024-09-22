using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.PaginationService;
using static TicketingApp.Services.AlertViewService;

namespace TicketingApp.Controllers;

public class ArtistController(TicketingAppCtx ctx) : Controller
{
    private readonly PaginationService<Artist> paginationService = new(ctx);
    public async Task<IActionResult> Index()
    {
        return View(await paginationService.ListAllPage(5));
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5, string template = "ArtistList")
    {
        if (string.IsNullOrEmpty(searchInput))
        {
            return PartialView(template, await paginationService.ListAllPage(pageSize));
        }

        return PartialView(template, await paginationService.Search(searchInput, pageSize));
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage, string template = "ArtistList")
    {
        return PartialView(template, await paginationService.Pagination(searchInput, offset, pageCount, pageSize, currentPage));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<string> Create(ArtistCreateDto artistCreateDto)
    {
        var errMessages = new List<string>();
        if (!ModelState.IsValid)
        {
            errMessages.AddRange(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList());
        }

        if (errMessages.Count != 0)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return SendAlert(AlertType.danger, string.Join(" | ", errMessages));
        }

        var newArtist = new Artist
        {
            Name = artistCreateDto.Name,
            Description = artistCreateDto.Description,
        };

        ctx.Add(newArtist);
        await ctx.SaveChangesAsync();

        HttpContext.Response.Headers.Append("HX-Redirect", "/artist");

        return "";
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
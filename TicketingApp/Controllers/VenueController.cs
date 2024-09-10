using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.PaginationService;
using static TicketingApp.Services.AlertViewService;

namespace TicketingApp.Controllers;

public class VenueController(TicketingAppCtx ctx) : Controller
{
    private readonly PaginationService<Venue> paginationService = new(ctx);
    public async Task<IActionResult> Index()
    {
        return View(await paginationService.ListAllPage(5));
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5)
    {
        if (string.IsNullOrEmpty(searchInput))
        {
            return PartialView("VenueList", await paginationService.ListAllPage(pageSize));
        }

        return PartialView("VenueList", await paginationService.Search(searchInput, pageSize));
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage)
    {
        return PartialView("VenueList", await paginationService.Pagination(searchInput, offset, pageCount, pageSize, currentPage));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<string> Create(VenueCreateDto venueCreateDto)
    {
        // TODO: move this to a service
        if (!ModelState.IsValid)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return SendAlert(AlertType.danger, "Bad request");
        }

        var newVenue = new Venue
        {
            Name = venueCreateDto.Name,
            Address = venueCreateDto.Address,
            Seats = []
        };

        // ctx.Venues.Add(newVenue);
        // ctx.Seats.AddRange(newVenue.Seats);

        // await ctx.SaveChangesAsync();

        // HttpContext.Response.Headers.Append("HX-Redirect", "/venue");
        return "";
    }

    public async Task<IActionResult> Edit(int id)
    {
        var venue = await ctx.Venues
            .Include(v => v.Seats)
            .FirstOrDefaultAsync(v => v.Id == id);

        return View(venue);
    }
}
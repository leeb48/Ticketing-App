using System.Net;
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
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5, string template = "VenueList")
    {
        if (string.IsNullOrEmpty(searchInput))
        {
            return PartialView(template, await paginationService.ListAllPage(pageSize));
        }

        return PartialView(template, await paginationService.Search(searchInput, pageSize));
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage, string template = "VenueList")
    {
        return PartialView(template, await paginationService.Pagination(searchInput, offset, pageCount, pageSize, currentPage));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<string> Create(VenueCreateDto venue)
    {
        // TODO: move this to a service
        if (!ModelState.IsValid)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return SendAlert(AlertType.danger, "Bad request");
        }

        var newVenue = new Venue
        {
            Name = venue.Name,
            Address = venue.Address,
            RowCount = venue.RowCountInput,
            ColumnCount = venue.ColCountInput,
            Seats = []
        };

        for (var row = 0; row < venue.RowCountInput; ++row)
        {
            for (var col = 0; col < venue.ColCountInput; ++col)
            {
                newVenue.Seats.Add(new Seat { Venue = newVenue, Row = row, Column = col });
            }
        }

        ctx.Venues.Add(newVenue);
        ctx.Seats.AddRange(newVenue.Seats);

        await ctx.SaveChangesAsync();

        HttpContext.Response.Headers.Append("HX-Redirect", "/venue");
        return "";
    }

    public async Task<IActionResult> Edit(int id)
    {
        var venue = await ctx.Venues
            .Include(v => v.Seats)
            .FirstOrDefaultAsync(v => v.Id == id);

        return View(venue);
    }

    [HttpPost]
    public async Task<string> Edit(VenueUpdateDto venue)
    {
        // TODO: make this into its own param validate service
        var errMessages = new List<string>();

        if (!ModelState.IsValid)
        {
            errMessages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
        }

        if (venue.Id == 0)
        {
            errMessages.Add($"Venue with ID {venue.Id} not found.");
        }

        if (errMessages.Count != 0)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return SendAlert(AlertType.danger, string.Join(" | ", errMessages));
        }

        await ctx.Venues
                .Where(v => v.Id == venue.Id)
                .ExecuteUpdateAsync(prop => prop
                    .SetProperty(v => v.Name, venue.Name)
                    .SetProperty(v => v.Address, venue.Address));

        HttpContext.Response.Headers.Append("HX-Redirect", "/venue");
        return "";
    }
}
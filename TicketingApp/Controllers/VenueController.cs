using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;
using static TicketingApp.Services.AlertViewService;

namespace TicketingApp.Controllers;

public class VenueController(TicketingAppCtx ctx) : Controller
{
    private readonly TicketingAppCtx ctx = ctx;

    public async Task<IActionResult> Index()
    {
        var venue = await ctx.Venues.Take(10).ToListAsync();

        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<string> Create(VenueCreateDto venueCreateDto)
    {
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

        var seatingChart = JsonSerializer.Deserialize<Dictionary<string, int>>(venueCreateDto.SeatingChart);

        foreach (var entry in seatingChart!)
        {
            for (var i = 0; i < entry.Value; ++i)
            {
                newVenue.Seats.Add(new Seat { Venue = newVenue, Row = entry.Key, Column = i.ToString() });
            }
        }

        ctx.Venues.Add(newVenue);
        ctx.Seats.AddRange(newVenue.Seats);

        await ctx.SaveChangesAsync();

        HttpContext.Response.Headers.Append("HX-Redirect", "/venue");
        return "";
    }
}
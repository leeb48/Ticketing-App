using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;
using static TicketingApp.Services.AlertViewService;

namespace TicketingApp.Controllers;

public class EventController(TicketingAppCtx ctx) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<string> Create(EventCreateDto eventCreateDto)
    {
        var errMessages = new List<string>();
        if (!ModelState.IsValid)
        {
            errMessages.AddRange(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList());
        }

        if (eventCreateDto.ArtistId == 0 || eventCreateDto.VenueId == 0)
        {
            errMessages.Add($"ArtistID: {eventCreateDto.ArtistId} or VenueID: {eventCreateDto.VenueId} is invalid");
        }

        var artist = await ctx.Artists.FirstOrDefaultAsync(a => a.Id == eventCreateDto.ArtistId);
        var venue = await ctx.Venues.FirstOrDefaultAsync(v => v.Id == eventCreateDto.VenueId);

        if (artist == null)
        {
            errMessages.Add($"Artist with ID: {eventCreateDto.ArtistId} not found");
        }

        if (venue == null)
        {
            errMessages.Add($"Venue with ID: {eventCreateDto.VenueId} not found");
        }

        if (errMessages.Count != 0)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return SendAlert(AlertType.danger, string.Join(" | ", errMessages));
        }

        var newEvent = new Event
        {
            Name = eventCreateDto.Name,
            Description = eventCreateDto.Description,
            Date = eventCreateDto.Date,
            Category = eventCreateDto.Category,
            Artist = artist!,
            Venue = venue!,
        };

        ctx.Events.Add(newEvent);
        await ctx.SaveChangesAsync();

        return "";
    }
}
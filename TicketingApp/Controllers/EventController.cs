using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.PaginationService;
using TicketingApp.Services.TicketLockService;
using static TicketingApp.Services.AlertViewService;

namespace TicketingApp.Controllers;

public class EventController(TicketingAppCtx ctx, ILockService<Ticket> lockService) : Controller
{
    private readonly PaginationService<Event> paginationService = new(ctx);
    public async Task<IActionResult> Index()
    {
        return View(await paginationService.ListAllPage(5));
    }

    [HttpPost]
    public async Task<IActionResult> Search(string searchInput, int pageSize = 5, string template = "EventList")
    {
        if (string.IsNullOrEmpty(searchInput))
        {
            return PartialView(template, await paginationService.ListAllPage(pageSize));
        }

        return PartialView(template, await paginationService.Search(searchInput, pageSize));
    }

    public async Task<IActionResult> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage, string template = "EventList")
    {
        return PartialView(template, await paginationService.Pagination(searchInput, offset, pageCount, pageSize, currentPage));
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

        var artist = await ctx.Artists.FirstOrDefaultAsync(a => a.Id == eventCreateDto.ArtistId);
        var venue = await ctx.Venues.Include(v => v.Seats).FirstOrDefaultAsync(v => v.Id == eventCreateDto.VenueId);

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
            Id = eventCreateDto.Id ?? 0,
            Name = eventCreateDto.Name,
            Description = eventCreateDto.Description,
            Date = eventCreateDto.Date,
            Category = eventCreateDto.Category,
            Artist = artist!,
            Venue = venue!,
        };

        ctx.Events.Update(newEvent);

        if (eventCreateDto.Id == null)
        {
            var tickets = new List<Ticket>();
            foreach (var seat in venue!.Seats)
            {
                var ticket = new Ticket { Event = newEvent, Status = TicketStatus.Available, Seat = seat };
                tickets.Add(ticket);
            }

            ctx.Tickets.AddRange(tickets);
        }

        await ctx.SaveChangesAsync();

        HttpContext.Response.Headers.Append("HX-Redirect", "/event");

        return "";
    }

    public async Task<IActionResult> Edit(int id)
    {
        var eventEntity = await ctx.Events
            .Include(e => e.Artist)
            .Include(e => e.Venue)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventEntity == null)
        {
            return View("Error", new ErrorViewModel { Message = $"Event with ID: {id} was not found" });
        }

        return View(eventEntity);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var eventEntity = await ctx.Events
            .Include(e => e.Artist)
            .Include(e => e.Venue)
            .Include(e => e.Tickets)
                .ThenInclude(t => t.Seat)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventEntity == null)
        {
            return View("Error", new ErrorViewModel { Message = $"Event with ID: {id} was not found" });
        }

        var lockedTickets = await lockService.GetLockedEntities(eventEntity!.Tickets);

        var reservedSeats = eventEntity.Tickets
            .Where(t => t.Status == TicketStatus.Reserved)
            .Select(t => t.Seat)
            .ToList();

        foreach (var ticket in lockedTickets)
        {
            reservedSeats.Add(ticket.Seat);
        }

        var eventDetailView = new EventDetailView
        {
            Event = eventEntity,
            ReservedSeats = reservedSeats,
        };

        return View(eventDetailView);
    }
}
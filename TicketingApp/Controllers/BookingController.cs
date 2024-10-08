using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.TicketLockService;
using static TicketingApp.Services.AlertViewService;


namespace TicketingApp.Controllers;

public class BookingController(TicketingAppCtx ctx, ILockService<Ticket> lockService, IConfiguration config) : Controller
{
    [HttpPost]
    public async Task<IActionResult> UpdateWH()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);

            Console.WriteLine(stripeEvent.Type);

            if (stripeEvent.Type == "charge.succeeded")
            {
                Console.WriteLine(stripeEvent.Data.Object);
            }
        }
        catch (StripeException e)
        {
            Console.WriteLine(e.Message);
            return BadRequest();
        }

        return Ok();
    }
    public async Task<IActionResult> Checkout(int id)
    {
        var booking = await ctx.Bookings
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Event)
                .ThenInclude(e => e.Artist)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Event)
                .ThenInclude(e => e.Venue)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
            .FirstOrDefaultAsync(b => b.Id == id);

        lockService.CreateLock(booking!.Tickets, 60);

        var options = new PaymentIntentCreateOptions
        {
            Amount = 100,
            Currency = "usd",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
            Metadata = new Dictionary<string, string> { { "bookingId", id.ToString() } }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        return View(new BookingViewModel
        {
            Booking = booking,
            StripeClientSecret = intent.ClientSecret,
            StripePublicKey = config.GetValue<string>("Stripe:PublicKey")!,
        });
    }

    [HttpPost]
    public async Task<string> Purchase(int id)
    {
        var booking = await ctx.Bookings
            .Include(b => b.Tickets)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return SendAlert(AlertType.danger, "Booking was not found");
        }

        foreach (var ticket in booking.Tickets)
        {
            ticket.Status = TicketStatus.Reserved;
        }

        booking.Status = BookingStatus.Booked;
        await ctx.SaveChangesAsync();

        HttpContext.Response.Headers.Append("HX-Redirect", "/");

        return SendAlert(AlertType.success, "Successfully Booked");
    }

    public IActionResult Confirm(int id)
    {

        return View();
    }

    [HttpPost]
    public async Task<string> Checkout(string checkoutReq)
    {
        // TODO: handle error here
        try
        {
            var checkoutDto = JsonSerializer.Deserialize<CheckoutDto>(checkoutReq);

            var tickets = new List<Ticket>();

            foreach (var seatIdx in checkoutDto!.Cart)
            {
                var ticket = await ctx.Tickets
                    .Include(t => t.Seat)
                    .Include(t => t.Event)
                    .FirstOrDefaultAsync(t => t.Event.Id == checkoutDto.EventId
                        && t.Seat.Row == seatIdx.Row
                        && t.Seat.Column == seatIdx.Col);

                if (ticket != null) tickets.Add(ticket);
            }

            var newUser = new User { Bookings = [] };
            var booking = new Booking
            {
                User = newUser,
                Tickets = tickets,
                Status = BookingStatus.Processing,
            };

            newUser.Bookings.Add(booking);

            await ctx.Users.AddAsync(newUser);
            await ctx.Bookings.AddAsync(booking);
            await ctx.SaveChangesAsync();

            HttpContext.Response.Headers.Append("HX-Redirect", $"/booking/checkout/{booking.Id}");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return "";
    }
}
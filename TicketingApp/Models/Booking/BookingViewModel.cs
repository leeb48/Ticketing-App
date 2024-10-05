namespace TicketingApp.Models;

public class BookingViewModel
{
    public required Booking Booking { get; set; }

    public required string StripeClientSecret { get; set; }

    public required string StripePublicKey { get; set; }
}
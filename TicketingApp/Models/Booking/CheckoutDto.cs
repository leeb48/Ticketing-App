namespace TicketingApp.Models;

public class Cart
{
    public int Row { get; set; }
    public int Col { get; set; }
}

public class CheckoutDto
{
    public int EventId { get; set; }
    public required List<Cart> Cart { get; set; }
}
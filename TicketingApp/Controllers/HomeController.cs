using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.PaginationService;

namespace TicketingApp.Controllers;

public class HomeController(ILogger<HomeController> logger, TicketingAppCtx ctx) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    private readonly PaginationService<Event> paginationService = new(ctx);

    public async Task<IActionResult> Index()
    {
        return View(await paginationService.ListAllPage(5));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

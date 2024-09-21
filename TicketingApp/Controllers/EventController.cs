using Microsoft.AspNetCore.Mvc;

namespace TicketingApp.Controllers;

public class EventController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }


}
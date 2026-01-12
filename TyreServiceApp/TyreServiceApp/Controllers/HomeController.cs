using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TyreServiceApp.Data;
using TyreServiceApp.Models;

namespace TyreServiceApp.Controllers;

/// <summary>
/// Контроллер для главной страницы и системных операций.
/// </summary>
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера.
    /// </summary>
    /// <param name="logger">Логгер для записи событий.</param>
    /// <param name="context">Контекст базы данных.</param>
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Возвращает главную страницу с аналитическими данными.
    /// </summary>
    public IActionResult Index()
    {
        ViewBag.ClientsCount = _context.Clients.Count();
        ViewBag.CarsCount = _context.Cars.Count();
        ViewBag.OrdersCount = _context.Orders.Count();
        ViewBag.ServicesCount = _context.Services.Count();
        ViewBag.MastersCount = _context.Masters.Count();
        ViewBag.TiresCount = _context.Tires.Count();
        ViewBag.CompletedWorksCount = _context.CompletedWorks.Count();

        var allOrders = _context.Orders.ToList();
        var ordersWithCompletedWorks = _context.CompletedWorks
            .Select(cw => cw.OrderNumber)
            .Distinct()
            .ToList();
        
        ViewBag.ActiveOrdersCount = allOrders
            .Where(o => !ordersWithCompletedWorks.Contains(o.OrderNumber))
            .Count();

        ViewBag.CompletedOrdersCount = ordersWithCompletedWorks.Count;

        ViewBag.TodayOrdersCount = _context.Orders
            .Where(o => o.OrderDate.Date == DateTime.Today)
            .Count();

        ViewBag.UnpaidOrdersCount = _context.Orders
            .Where(o => o.PaymentDate == null)
            .Count();

        ViewBag.OrdersWithMastersCount = _context.Orders
            .Where(o => o.MasterId != null)
            .Count();

        var topClients = _context.Clients
            .Select(c => new
            {
                Client = c,
                OrderCount = _context.Cars
                    .Where(car => car.ClientId == c.ClientId)
                    .SelectMany(car => car.Orders)
                    .Count()
            })
            .OrderByDescending(x => x.OrderCount)
            .Take(3)
            .ToList();

        ViewBag.TopClients = topClients;

        ViewBag.RecentOrders = _context.Orders
            .Include(o => o.Car)
                .ThenInclude(c => c.Client)
            .Include(o => o.Master)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .ToList();

        return View();
    }

    /// <summary>
    /// Возвращает страницу политики конфиденциальности.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Обрабатывает и отображает страницу ошибок.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
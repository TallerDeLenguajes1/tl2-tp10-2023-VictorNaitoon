using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP10.Models;
using TP10.Repository;

namespace TP10.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IUsuarioRepository _repository;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _repository = new UsuarioRepository();
    }

    public IActionResult Index()
    {
        return View(_repository.GetAllUsuarios());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

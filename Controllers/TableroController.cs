using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TP10.Models;
using TP10.Repository;

namespace TP10.Controllers
{
    [Route("[controller]")]
    public class TableroController : Controller
    {
        private readonly ILogger<TableroController> _logger;
        private ITableroRepository _repository;

        public TableroController(ILogger<TableroController> logger)
        {
            _logger = logger;
            _repository = new TableroRepository();
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        [HttpGet("CreateTablero")]
        public IActionResult CreateTablero()
        {
            return View(new Tablero());
        }

        [HttpPost("CreateTablero")]
        public IActionResult CreateTablero(Tablero tablero)
        {
            _repository.Create(tablero);
            return RedirectToAction("Index");
        }

        [HttpGet("UpdateTablero")]
        public IActionResult UpdateTablero(int idTablero)
        {
            return View(_repository.Get(idTablero));
        }

        [HttpPost("UpdateTablero")]
        public IActionResult UpdateTablero(Tablero tablero)
        {
            _repository.Update(tablero.IdTablero, tablero);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTablero(int idTablero)
        {
            _repository.Delete(idTablero);
            return RedirectToAction("Index");
        }

    }
}
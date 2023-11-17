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
    public class TareaController : Controller
    {
        private readonly ILogger<TareaController> _logger;
        private ITareaRepository _repository;

        public TareaController(ILogger<TareaController> logger)
        {
            _logger = logger;
            _repository = new TareaRepository();
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(_repository.GetAllTareas());
        }

        [HttpGet("CreateTarea")]
        public IActionResult CreateTarea()
        {
            return View(new Tarea());
        }

        [HttpPost("CreateTarea")]
        public IActionResult CreateTarea(Tarea tarea)
        {
            _repository.Create(tarea.IdTablero, tarea);
            return RedirectToAction("Index");
        }

        [HttpGet("UpdateTarea")]
        public IActionResult UpdateTarea(int idTarea)
        {
            return View(_repository.Get(idTarea));
        }

        [HttpPost("UpdateTarea")]
        public IActionResult UpdateTarea(Tarea tarea)
        {
            _repository.Update(tarea.IdTarea, tarea);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteTarea(int idTarea)
        {
            _repository.Delete(idTarea);
            return RedirectToAction("Index");
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TP10.Models;
using TP10.Repository;
using TP10.ViewModels;

namespace TP10.Controllers
{
    [Route("[controller]")]
    public class TableroController : Controller
    {
        private readonly ILogger<TableroController> _logger;
        private ITableroRepository _repository;
        private IUsuarioRepository _usuarioRepositorio;

        public TableroController(ILogger<TableroController> logger, ITableroRepository repository, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _repository = repository;
            _usuarioRepositorio = usuarioRepository;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            try
            {
                if(isLogin())
                {
                    if(isAdmin())
                    {
                        var tableros = _repository.GetAll();
                        var tablerosViewModel = tableros.Select(t => new ListarTablerosViewModel(t, _usuarioRepositorio)).ToList();
                        return View(tablerosViewModel);
                    }

                    var tablerosPorId = _repository.GetAllTableros(Convert.ToInt32(HttpContext.Session.GetString("Id")));
                    var tablerosVM = tablerosPorId.Select(x => new ListarTablerosViewModel(x, _usuarioRepositorio)).ToList();
                    return View(tablerosVM);
                }   
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar listar los tableros: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("CreateTablero")]
        public IActionResult CreateTablero()
        {
            try
            {  
                if(isLogin())
                {
                    return View(new AñadirTableroViewModel());
                } 
                return RedirectToAction("Index", "Tablero");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar crear un tablero: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("CreateTablero")]
        public IActionResult CreateTablero(AñadirTableroViewModel tablero)
        {
            try
            {
                if(!ModelState.IsValid) return View("CreateTablero", tablero);

                var tableroNuevo = new Tablero();
                tableroNuevo.Nombre = tablero.Nombre;
                tableroNuevo.IdUsuarioPropietario = tablero.IdUsuarioPropietario;
                tableroNuevo.Descripcion = tablero.Descripcion;

                _repository.Create(tableroNuevo);

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar crear un tablero: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar crear el tablero. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("UpdateTablero")]
        public IActionResult UpdateTablero(int idTablero)
        {
            try
            {
                if(isLogin() && isAdmin())
                {
                    return View(new ModificarTableroViewModel(_repository.Get(idTablero)));
                }
                return RedirectToAction("Index", "Tablero");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar modificar un usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("UpdateTablero")]
        public IActionResult UpdateTablero(ModificarTableroViewModel tablero)
        {
            try
            {
                if(!ModelState.IsValid) return View("UpdateTablero", tablero);

                var tableroNuevo = new Tablero();
                tableroNuevo.Nombre = tablero.Nombre;
                tableroNuevo.Descripcion = tablero.Descripcion;
                tableroNuevo.IdUsuarioPropietario = tablero.IdUsuarioPropietario;

                _repository.Update(tablero.Id, tableroNuevo);

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar modificar un tablero: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar modificar el tablero. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("DeleteTablero")]
        public IActionResult DeleteTablero(int idTablero)
        {
            try
            {
                if(isLogin() && isAdmin())
                {
                    _repository.Delete(idTablero);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar eliminar un tablero: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }


        //Funciones para login y administrador
        public bool isLogin()
        {
            return HttpContext.Session != null && HttpContext.Session.Keys.Any();
        }

        public bool isAdmin()
        {
            return HttpContext.Session.GetString("Rol") == Enum.GetName(Roles.Administrador);
        }

    }
}
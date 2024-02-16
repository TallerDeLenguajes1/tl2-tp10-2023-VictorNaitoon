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
    public class TareaController : Controller
    {
        private readonly ILogger<TareaController> _logger;
        private ITareaRepository _repository;
        private ITableroRepository _tableroRepositorio;
        private IUsuarioRepository _usuarioRepositorio;

        public TareaController(ILogger<TareaController> logger, ITareaRepository repository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _repository = repository;
            _tableroRepositorio = tableroRepository;
            _usuarioRepositorio = usuarioRepository;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            try
            {
                if(isLogin())
                {
                    var tareas = _repository.GetAllTareas();
                    var tareasVM = tareas.Select(x => new ListarTareasViewModel(x, _tableroRepositorio, _usuarioRepositorio)).ToList();
                    return View(tareasVM);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar listar las tareas: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("CreateTarea")]
        public IActionResult CreateTarea()
        {
            try
            {   
                if(isLogin() && isAdmin())
                {
                    var usuarios = _usuarioRepositorio.GetAllUsuarios().ToList();
                    var tableros = _tableroRepositorio.GetAll().ToList();

                    var añadirVM = new AñadirTareaViewModel();
                    añadirVM.Inicializar(tableros, usuarios);
                    return View(añadirVM);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar crear una tarea: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("CreateTarea")]
        public IActionResult CreateTarea(AñadirTareaViewModel tarea)
        {
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("CreateTarea", tarea);
            
                var tareaNueva = new Tarea();
                tareaNueva.IdTablero = tarea.IdTablero;
                tareaNueva.Nombre = tarea.Nombre;
                tareaNueva.Estado = tarea.Estado;
                tareaNueva.Descripcion = tarea.Descripcion;
                tareaNueva.Color = tarea.Color;
                tareaNueva.IdUsuarioAsignado = tarea.IdUsuarioAsignado;

                _repository.Create(tareaNueva.IdTablero, tareaNueva);

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar crear una tarea: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar crear una tarea. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("UpdateTarea")]
        public IActionResult UpdateTarea(int idTarea)
        {
            try
            {
                if(isLogin())
                {
                    var usuarios = _usuarioRepositorio.GetAllUsuarios();
                    var tableros = _tableroRepositorio.GetAll();
                    var tareaAModificar = _repository.Get(idTarea);

                    var modificarTareaVM = new ModificarTareaViewModel(tareaAModificar, tableros, usuarios);

                    return View(modificarTareaVM);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar modificar una tarea: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("UpdateTarea")]
        public IActionResult UpdateTarea(ModificarTareaViewModel tarea)
        {
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("UpdateTarea", tarea);
            
                var nuevaTarea = new Tarea();
                nuevaTarea.IdTablero = tarea.IdTablero;
                nuevaTarea.Nombre = tarea.Nombre;
                nuevaTarea.Descripcion = tarea.Descripcion;
                nuevaTarea.Estado = tarea.Estado;
                nuevaTarea.Color = tarea.Color;
                nuevaTarea.IdUsuarioAsignado = tarea.IdUsuarioAsignado;

                _repository.Update(tarea.IdTarea, nuevaTarea);

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar modificar una tarea: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar modificar una tarea. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("DeleteTarea")]
        public IActionResult DeleteTarea(int idTarea)
        {
            try
            {
                if(isLogin())
                {
                    _repository.Delete(idTarea);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "Tarea");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar eliminar una tarea: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("AsignarUsuario")]
        public IActionResult AsignarUsuario(int idTarea)
        {
            try
            {
                if(isLogin())
                {
                    var usuarios = _usuarioRepositorio.GetAllUsuarios();
                    var asignarVM = new AsignarUsuarioViewModel(idTarea, usuarios);

                    return View(asignarVM);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar asignar una tarea a otro usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("AsignarUsuario")]
        public IActionResult AsignarUsuario(AsignarUsuarioViewModel tarea)
        {
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("AsignarUsuario", tarea);

                _repository.AsignarUsuarioATarea(tarea.IdUsuarioAsignado, tarea.IdTarea);

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar asignar una tarea a otro usuario: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar asignar una tarea a otro usuario. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
        }



        //Funciones para verificar si hay una session activa y si es administrador el usuario
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
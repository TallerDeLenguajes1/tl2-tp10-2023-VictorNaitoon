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
                    var tableros = _tableroRepositorio.GetAll();
                    var tareas = _repository.GetAllTareas();
                    var listarVM = new ListarTareasViewModel();
                    listarVM.Inicializar(tableros, tareas);
                    return View(listarVM);
                }
                TempData["MensajeDeLogueo"] = "Debes loguearta para acceder al sistema";
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar acceder al index: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("TareasPorTableros")]
        public IActionResult TareasPorTableros(int idTablero)
        {
            try
            {
                if(isLogin())
                {
                    var tareas = _repository.GetAllPorTablero(idTablero);
                    var tableros = _tableroRepositorio.GetAll();
                    var usuarios = _usuarioRepositorio.GetAllUsuarios();

                    if(isAdmin())
                    {
                        tareas = _repository.GetAllTareas().Where(t => t.IdTablero == idTablero).ToList();
                    } else {
                        tareas = tareas.Where(x => x.IdUsuarioAsignado == Convert.ToInt32(HttpContext.Session.GetString("Id"))).ToList();
                    }

                    return View(new ListarTareasViewModel(idTablero, tableros, tareas, usuarios));
                }
                TempData["MensajeDeLogueo"] = "Debes loguearta para acceder al sistema";
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar listar las tareas: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost("SeleccionarTablero")]
        public IActionResult SeleccionarTablero(int idTablero)
        {
            try
            {
                if(isLogin())
                {
                    return RedirectToAction("TareasPorTableros", new { idTablero = idTablero });
                }
                TempData["MensajeDeLogueo"] = "Debes loguearta para acceder al sistema";
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrió un error al intentar seleccionar el tablero: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("TareasSinAsignar")]
        public IActionResult TareasSinAsignar()
        {
            try
            {
                if(isLogin())
                {
                    if(isAdmin())
                    {
                        var tareas = _repository.GetAllTareas().Where(x => x.IdUsuarioAsignado == 0).ToList();
                        var usuarios = _usuarioRepositorio.GetAllUsuarios();
                        var listarVM = new ListarTareasViewModel();
                        listarVM.InicializarTareas(tareas, usuarios);
                        return View(listarVM);
                    }
                    TempData["MensajeDeAlerta"] = "No puedes acceder, no eres Administrador";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearta para acceder al sistema";
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrió un error al intentar listar las tareas sin asignar: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet("CreateTarea")]
        public IActionResult CreateTarea()
        {
            try
            {   
                if(isLogin())
                {
                    var usuarios = _usuarioRepositorio.GetAllUsuarios().ToList();
                    var tableros = _tableroRepositorio.GetAll().ToList();

                    var añadirVM = new AñadirTareaViewModel();
                    añadirVM.Inicializar(tableros, usuarios);
                    return View(añadirVM);
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
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
                if(isLogin())
                {
                    if(!ModelState.IsValid) return RedirectToAction("CreateTarea", tarea);
                    var tareaNueva = new Tarea();

                    if(isAdmin())
                    {
                        tareaNueva.IdTablero = tarea.IdTablero;
                        tareaNueva.Nombre = tarea.Nombre;
                        tareaNueva.Estado = tarea.Estado;
                        tareaNueva.Descripcion = tarea.Descripcion;
                        //tareaNueva.Color = ObtenerColor(tareaNueva.Estado);
                        tareaNueva.IdUsuarioAsignado = tarea.IdUsuarioAsignado;

                        
                        tareaNueva.Color = tarea.Color;
                        

                        _repository.Create(tareaNueva.IdTablero, tareaNueva);

                        return RedirectToAction("Index");
                    }
                    tareaNueva.IdTablero = tarea.IdTablero;
                    tareaNueva.Nombre = tarea.Nombre;
                    tareaNueva.Estado = tarea.Estado;
                    tareaNueva.Descripcion = tarea.Descripcion;
                    //tareaNueva.Color = ObtenerColor(tareaNueva.Estado);
                    tareaNueva.IdUsuarioAsignado = Convert.ToInt32(HttpContext.Session.GetString("Id"));

                    
                    tareaNueva.Color = tarea.Color;
                    

                    _repository.Create(tareaNueva.IdTablero, tareaNueva);

                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToAction("Index", "Login");
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

                    if(tareaAModificar.IdUsuarioAsignado == Convert.ToInt32(HttpContext.Session.GetString("Id")) || isAdmin())
                    {
                        var modificarTareaVM = new ModificarTareaViewModel(tareaAModificar, tableros, usuarios);

                        return View(modificarTareaVM);
                    }
                    TempData["MensajeDeAlerta"] = "No puedes modificar una tarea que no te pertenece o no eres Administrador";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
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
                if(isLogin())
                {
                    if(!ModelState.IsValid) return RedirectToAction("UpdateTarea", tarea);
                
                    var nuevaTarea = new Tarea();
                    nuevaTarea.IdTablero = tarea.IdTablero;
                    nuevaTarea.Nombre = tarea.Nombre;
                    nuevaTarea.Descripcion = tarea.Descripcion;
                    nuevaTarea.Estado = tarea.Estado;
                    //nuevaTarea.Color = ObtenerColor(nuevaTarea.Estado);
                    nuevaTarea.IdUsuarioAsignado = tarea.IdUsuarioAsignado;

                    
                    nuevaTarea.Color = tarea.Color;
                    

                    _repository.Update(tarea.IdTarea, nuevaTarea);

                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToAction("Index", "Login");
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
                    var tareaEliminar = _repository.Get(idTarea);

                    if(tareaEliminar.IdTarea == Convert.ToInt32(HttpContext.Session.GetString("Id")) || isAdmin())
                    {
                        _repository.Delete(idTarea);
                        return RedirectToAction("Index");
                    }
                    TempData["MensajeDeAlerta"] = "No puede eliminar esta tarea por que no te pertenece o no eres Administradir";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar eliminar una tarea: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }


        //FALTA REVISAR EL ASIGNAR USUARIO
        [HttpGet("AsignarUsuario")]
        public IActionResult AsignarUsuario(int idTarea)
        {
            try
            {
                if(isLogin())
                {
                    if(isAdmin())
                    {
                        var tareaObtenida = _repository.Get(idTarea);
                        var usuarios = _usuarioRepositorio.GetAllUsuarios();
                        var asignarVM = new AsignarUsuarioViewModel(idTarea, usuarios);
                        
                        return View(asignarVM);
                    }
                    TempData["MensajeDeAlerta"] = "No puedes asignar tareas a usuarios por que no eres administrador";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes logearte para poder ingresar";
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
                if(isLogin())
                {
                    if(isAdmin())
                    {
                        if(!ModelState.IsValid) return RedirectToAction("AsignarUsuario", tarea);

                        _repository.AsignarUsuarioATarea(tarea.IdUsuarioAsignado, tarea.IdTarea);

                        return RedirectToAction("Index");
                    }
                    TempData["MensajeDeAlerta"] = "No puedes asignar un usuario a una tarea por que no eres administrador";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToAction("Index", "Login");
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
        private string ObtenerColor(EstadoTarea Estado)
        {
            switch (Estado)
            {
                case EstadoTarea.Ideas:
                return "yellow";
                case EstadoTarea.ToDo:
                return "skyblue";
                case EstadoTarea.Doing:
                return "red";
                case EstadoTarea.Review:
                return "blue";
                default:
                return "green";
            }
        }
    }
}
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
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private IUsuarioRepository _repository;
        private ITareaRepository _repositoryTarea;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository repository, ITareaRepository repositoryTarea)
        {
            _logger = logger;
            _repository = repository;
            _repositoryTarea = repositoryTarea;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            try
            {
                if(isLogin())
                {
                    return View(new ListarUsuariosViewModels(_repository.GetAllUsuarios(), isAdmin()));
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToRoute(new { controller = "Login", action = "Index"});
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new { controller = "Home", action = "Error"});
            }
        }

        
        [HttpGet("CreateUsuario")]
        public IActionResult CreateUsuario()
        {
            
            try
            {
                return View(new AñadirUsuarioViewModel()); 
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar crear un usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("CreateUsuario")]
        public IActionResult CreateUsuario(AñadirUsuarioViewModel usuario)
        {
            try
            {
                if(!ModelState.IsValid) return View("CreateUsuario", usuario);
                    
                var usuarioComprobar = _repository.ExisteNombre(usuario.NombreUsuario);
                if(usuarioComprobar.NombreDeUsuario != null)
                {
                    usuario.MensajeDeError = $"El nombre {usuario.NombreUsuario} no esta disponible";
                    return View("CreateUsuario", usuario);
                }

                var user = new Usuario();
                if(HttpContext.Session.GetString("Rol") == Roles.Administrador.ToString())
                {
                    user.NombreDeUsuario = usuario.NombreUsuario;
                    user.Rol = usuario.Rol;
                    user.Contrasenia = usuario.Contrasenia;
                    _repository.CreateUsuario(user);
                            
                    return RedirectToAction("Index");
                }
                user.NombreDeUsuario = usuario.NombreUsuario;
                user.Contrasenia = usuario.Contrasenia;
                user.Rol = Roles.Operador;

                _repository.CreateUsuario(user);
                            
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar crear un usuario: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar crear el usuario. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
            
        }

        [HttpGet("UpdateUsuario")]
        public IActionResult UpdateUsuario(int idUsuario)
        {
            try
            {
                if(isLogin())
                {
                    if(isAdmin() || idUsuario == Convert.ToInt32(HttpContext.Session.GetString("Id")))
                    {
                        return View(new ModificarUsuarioViewModel(_repository.GetUsuario(idUsuario)));
                    }
                    TempData["MensajeDeAlerta"] = "Debes ser administrador para modificar usuarios o debes ser el usuario que quieres modificar";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToAction("Index", "Usuario");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar modificar un usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }   
        }

        [HttpPost("UpdateUsuario")]
        public IActionResult UpdateUsuario(ModificarUsuarioViewModel usuario)
        {
            try
            {

                if(isLogin())
                {
                    if(!ModelState.IsValid) return RedirectToAction("UpdateUsuario", usuario);
                    var user = new Usuario();   

                    var usuarioComprobar = _repository.ExisteNombre(usuario.NombreUsuario);
                    if(usuarioComprobar.NombreDeUsuario != null)
                    {
                        usuario.MensajeDeError = $"El nombre {usuario.NombreUsuario} no esta disponible";
                        return View("UpdateUsuario", usuario);
                    }
                    
                    if(isAdmin())
                    {
                        user.NombreDeUsuario = usuario.NombreUsuario;
                        user.Contrasenia = usuario.Contrasenia;
                        user.Rol = usuario.Rol;
                        _repository.UpdateUsuario(usuario.Id, user);

                        return RedirectToAction("Index");
                    }

                    user.NombreDeUsuario = usuario.NombreUsuario;
                    user.Contrasenia = usuario.Contrasenia;
                    user.Rol = Roles.Operador;
                    _repository.UpdateUsuario(usuario.Id, user);

                    return RedirectToAction("Index");

                }
                TempData["MensajeDeLogueo"] = "Debes loguearta para acceder al sistema";
                return RedirectToAction("Index", "Login");
                
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar modificar un usuario: {ex.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrio un error al intentar modificar el usuario. Por favor, intentalo nuevamente.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("DeleteUsuario")]
        public IActionResult DeleteUsuario(int idUsuario)
        {
            try
            {
                if(isLogin())
                {
                    if(isAdmin() || idUsuario == Convert.ToInt32(HttpContext.Session.GetString("Id")))
                    {
                        _repository.DeleteUsuario(idUsuario);
                        _repositoryTarea.UpdateId(idUsuario);

                        if(idUsuario == Convert.ToInt32(HttpContext.Session.GetString("Id")))
                        {
                            return RedirectToAction("Logout", "Login");
                        }
                        return RedirectToAction("Index");
                    }
                    TempData["MensajeDeAlerta"] = "Debes ser administrador para eliminar usuarios o debe ser tu usuario el que quieres eliminar";
                    return RedirectToAction("Index");
                }
                TempData["MensajeDeLogueo"] = "Debes loguearte para acceder al sistema";
                return RedirectToAction("Index", "Login");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error al intentar eliminar un usuario: {ex.ToString()}");
                return RedirectToAction("Error", "Home");
            }
        }


        //Funciones para ver si hay una sesion activa y si el usuario es admirador
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
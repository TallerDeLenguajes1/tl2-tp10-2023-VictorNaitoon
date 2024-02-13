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

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository repository)
        {
            _logger = logger;
            _repository = repository;
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
                if(isLogin() && isAdmin()) return View(new AñadirUsuarioViewModel());
                return RedirectToAction("Index", "Usuario");
                
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

                var user = new Usuario();
                user.NombreDeUsuario = usuario.NombreUsuario;
                user.Rol = usuario.Rol;
                user.Contrasenia = usuario.Contrasenia;
                    
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
                if(isLogin() && isAdmin()) return View(new ModificarUsuarioViewModel(_repository.GetUsuario(idUsuario)));
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
                if(!ModelState.IsValid) return RedirectToAction("UpdateUsuario", usuario);
                var user = new Usuario();
                user.NombreDeUsuario = usuario.NombreUsuario;
                user.Contrasenia = usuario.Contrasenia;
                user.Rol = usuario.Rol;

                _repository.UpdateUsuario(usuario.Id, user);

                return RedirectToAction("Index");
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
                if(isLogin() && isAdmin())
                {
                    _repository.DeleteUsuario(idUsuario);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
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
        
        //TERMINADO LO DE USUARIO CON TODOS LOS VIEW MODELS, QUEDAN TABLEROS Y TAREAS
        

        //Para que se mande el id en la modificacion siempre tiene que ir con el hidder para que no nos muestre el id
        // Esto de comentar estas lineas provoco que desaparezca el error de los multiples endpoints
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}
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
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private IUsuarioRepository _repository;

        public LoginController(ILogger<LoginController> logger, IUsuarioRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("Login")]
        public IActionResult Login(Usuario usuario)
        {
            try
            {
                var usuarioLogueado = _repository.AutenticarUsuario(usuario.NombreDeUsuario, usuario.Contrasenia);

                if(usuarioLogueado == null || !VerificarCredenciales(usuario, usuarioLogueado))
                {
                    var loginVM = new LoginViewModel();
                    loginVM.MensajeDeError = "Usuario y/o contraseña incorrecto";

                    return View("Index", loginVM);

                } 
                loguearUsuario(usuarioLogueado);
                _logger.LogInformation($"El usuario {usuario.NombreDeUsuario} ingreso correctamente");
                return RedirectToRoute(new { controller = "Home", action = "Index"});
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error durante el intento de acceso con el usuario {ex.ToString()}");
                var loginVM = new LoginViewModel();
                loginVM.MensajeDeError = "Ocurrió un error al intentar inicar sesión. Por favor, intentalo nuevamente";
                return View("Index", loginVM);
            }
        }

        public void loguearUsuario(Usuario usuario)
        {
            HttpContext.Session.SetString("Id", usuario.Id.ToString());
            HttpContext.Session.SetString("NombreDeUsuario", usuario.NombreDeUsuario);
            HttpContext.Session.SetString("Contrasenia", usuario.Contrasenia);
            HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
        }

        private bool VerificarCredenciales(Usuario usuarioIngresado, Usuario usuarioAlmacenado)
        {
            return usuarioIngresado.NombreDeUsuario.ToLower() == usuarioAlmacenado.NombreDeUsuario.ToLower() && usuarioIngresado.Contrasenia == usuarioAlmacenado.Contrasenia;
        }
    }
}
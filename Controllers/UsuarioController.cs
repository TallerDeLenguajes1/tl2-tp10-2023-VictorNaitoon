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
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private IUsuarioRepository _repository;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
            _repository = new UsuarioRepository();
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(_repository.GetAllUsuarios());
        }

        
        [HttpGet("CreateUsuario")]
        public IActionResult CreateUsuario()
        {
            return View(new Usuario());
        }

        [HttpPost("CreateUsuario")]
        public IActionResult CreateUsuario(Usuario usuario)
        {
            _repository.CreateUsuario(usuario);
            return RedirectToAction("Index");
            //Llamamos al metodo que construye la vista, eso hace RedirectToAction.
        }

        [HttpGet("UpdateUsuario")]
        public IActionResult UpdateUsuario(int idUsuario)
        {
            return View(_repository.GetUsuario(idUsuario));
        }

        [HttpPost("UpdateUsuario")]
        public IActionResult UpdateUsuario(Usuario usuario)
        {
            //Cambiar algunas cosas del repositorio
            _repository.UpdateUsuario(usuario.Id, usuario);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteUsuario(int idUsuario)
        {
            _repository.DeleteUsuario(idUsuario);
            return RedirectToAction("Index");
        }
        

        

        //Para que se mande el id en la modificacion siempre tiene que ir con el hidder para que no nos muestre el id
        // Esto de comentar estas lineas provoco que desaparezca el error de los multiples endpoints
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}
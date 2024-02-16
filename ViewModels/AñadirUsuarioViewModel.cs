using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class AñadirUsuarioViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La contraseña debe ser de entre 8 y 30 caracteres")]
        public string Contrasenia { get; set; }
        public Roles Rol { get; set; }
    }
}
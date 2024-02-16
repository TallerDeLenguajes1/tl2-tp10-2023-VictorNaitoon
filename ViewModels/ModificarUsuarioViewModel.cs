using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class ModificarUsuarioViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La contrasenia debe tener entre 8 y 30 digitos")]
        public string Contrasenia { get; set; }
        public Roles? Rol { get; set; }

        public ModificarUsuarioViewModel()
        {
            
        }

        public ModificarUsuarioViewModel(Usuario user)
        {
            Id = user.Id;
            NombreUsuario = user.NombreDeUsuario;
            Contrasenia = user.Contrasenia;
            Rol = user.Rol;
        }
    }
}
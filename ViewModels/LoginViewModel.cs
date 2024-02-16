using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TP10.ViewModels
{
    public class LoginViewModel
    {
        public string MensajeDeError;
        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Nombre de usuario")]
        public string NombreDeUsuario { get; set; }
        [Required(ErrorMessage = "Este campo es requerida")]
        [PasswordPropertyText]
        [Display(Name = "Contraseña")]
        public string Contrasenia { get; set; }

        public bool TieneMensajeDeError => !string.IsNullOrEmpty(MensajeDeError);
    }
}
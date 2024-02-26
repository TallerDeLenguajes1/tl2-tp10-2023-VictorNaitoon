using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class AñadirTableroViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre del tablero")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripcion del tablero")]
        public string Descripcion { get; set; }
        public int IdUsuarioPropietario { get; set; }
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public string MensajeDeError;

        public bool TieneMensajeDeError => !string.IsNullOrEmpty(MensajeDeError);

        public void Inicializar(List<Usuario> usuarios)
        {
            Usuarios = usuarios;
        }
    }
}
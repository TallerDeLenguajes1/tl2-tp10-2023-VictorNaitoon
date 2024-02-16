using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [Required(ErrorMessage = "El id del usuario propietario es requerido")]
        [Display(Name = "Id del usuario propietario")]
        public int IdUsuarioPropietario { get; set; }
    }
}
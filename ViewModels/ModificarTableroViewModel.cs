using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class ModificarTableroViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre del tablero")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuarioPropietario { get; set; }

        public ModificarTableroViewModel()
        {
            
        }

        public ModificarTableroViewModel(Tablero tablero)
        {
            Id = tablero.IdTablero;
            Nombre = tablero.Nombre;
            IdUsuarioPropietario = tablero.IdUsuarioPropietario;
            Descripcion = tablero.Descripcion;
        }
    }
}
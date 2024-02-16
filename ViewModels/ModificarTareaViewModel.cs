using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class ModificarTareaViewModel
    {
        public int IdTarea { get; set; }
        [Required(ErrorMessage = "El id del tablero es requerido")]
        [Display(Name = "Id del tablero")]
        public int IdTablero { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre de tarea")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado de tarea")]
        public EstadoTarea Estado { get; set; }
        public string Color { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<Tablero> Tableros { get; set; } = new List<Tablero>();

        public ModificarTareaViewModel()
        {
            
        }

        public ModificarTareaViewModel(Tarea tarea, List<Tablero> tableros, List<Usuario> usuarios)
        {
            IdTarea = tarea.IdTarea;
            IdTablero = tarea.IdTablero;
            Nombre = tarea.Nombre;
            Descripcion = tarea.Descripcion;
            Estado = tarea.Estado;
            Color = tarea.Color;
            IdUsuarioAsignado = tarea.IdUsuarioAsignado;
            Tableros = tableros;
            Usuarios = usuarios;
        }
        
    }
}
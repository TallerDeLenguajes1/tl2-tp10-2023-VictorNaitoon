using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;
using TP10.Repository;

namespace TP10.ViewModels
{
    public class AñadirTareaViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Id del tablero")]
        public int IdTablero { get; set; }
        [Required(ErrorMessage = "El nombre de la tarea es requerida")]
        [Display(Name = "Nombre de tarea")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado de la tarea")]
        public EstadoTarea Estado { get; set; }
        public string Color { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public List<Tablero> Tableros { get; set; } = new List<Tablero>();
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();


        public AñadirTareaViewModel()
        {
            
        }
        public void Inicializar(List<Tablero> tableros, List<Usuario> usuarios)
        {
            Tableros = tableros;
            Usuarios = usuarios;
        }
    }
}
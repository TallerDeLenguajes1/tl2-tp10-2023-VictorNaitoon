using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;
using TP10.Repository;

namespace TP10.ViewModels
{
    public class ListarTareasViewModel
    {
        public int IdTarea { get; set; }
        public string TableroAlQuePertenece { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public EstadoTarea Estado { get; set; }
        public string UsuarioAsignado { get; set; }

        public ListarTareasViewModel()
        {
            
        }

        public ListarTareasViewModel(Tarea tarea, ITableroRepository _tableroRepositorio, IUsuarioRepository _usuarioRepositorio)
        {
            IdTarea = tarea.IdTarea;
            TableroAlQuePertenece = _tableroRepositorio.Get(tarea.IdTablero).Nombre;
            Nombre = tarea.Nombre;
            Descripcion = tarea.Descripcion;
            Color = tarea.Color;
            Estado = tarea.Estado;
            UsuarioAsignado = _usuarioRepositorio.GetUsuario(tarea.IdUsuarioAsignado).NombreDeUsuario;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class AsignarUsuarioViewModel
    {
        public int IdTarea { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public AsignarUsuarioViewModel(int idTarea, List<Usuario> usuarios)
        {
            IdTarea = idTarea;
            Usuarios = usuarios;
        }

        public AsignarUsuarioViewModel()
        {
            
        }
    }
}
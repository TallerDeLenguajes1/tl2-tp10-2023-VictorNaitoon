using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;
using TP10.Repository;

namespace TP10.ViewModels
{
    public class ListarTablerosViewModel
    {
        public int IdTablero { get; set; }
        public string NombreDeUsuarioPropietario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ListarTablerosViewModel()
        {
            
        }

        public ListarTablerosViewModel(Tablero tablero, IUsuarioRepository _repositorio)
        {
            IdTablero = tablero.IdTablero;
            NombreDeUsuarioPropietario = _repositorio.GetUsuario(tablero.IdUsuarioPropietario).NombreDeUsuario;
            Nombre = tablero.Nombre;
            Descripcion = tablero.Descripcion;
        }
    }
}
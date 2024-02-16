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


/*
Una idea que se me cae en este instante es que en el apartado c) El usuario logueado debe poder asignar un usuario a las tareas de las que es propietario. Aqui solo pueden asignar tareas los administradores que son los que van a crear las tareas, pero solo podran asignar usuarios a tareas que cuyo id del usuario administrador sea igual a id del que creo la tarea. 
A partir de esto lo que se me ocurre es que cuando yo agregue una nueva tarea que no ponga para llenar ese punto en el formulario ya que el id lo tenemos que sacar del id que tenga el httpcontext.

En el punto d. El usuario logueado debería poder ver en la lista de tableros, además de los
tableros que le pertenecen, todos los tableros donde tenga tareas asignadas.
Los permisos del usuario logueado para tableros que no le pertenecen son:
i. Tableros: Solo lectura
ii. Tareas no asignadas: Solo lectura.
iii. Tareas asignadas: Lectura y modificación, pero únicamente para
modificar el estado de la tarea.

ESTO ES APARTE
Lo que se me ocurrio es que todos puedan ver la lista grande de tableros que hay y que en cada tablero tenga un enlace en el que diga ver tareas, que son las distintas tareas que tendra cada tablero. Y a partir de ahi hacer que todos puedan ver las tareas pero

*/
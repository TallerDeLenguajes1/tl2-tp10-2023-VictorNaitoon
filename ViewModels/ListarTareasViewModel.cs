using TP10.Models;

namespace TP10.ViewModels
{
    public class ListarTareasViewModel
    {
        public List<Tarea> Tareas { get; set; }
        public int IdTablero { get; set; }
        public List<Tablero> Tableros { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public ListarTareasViewModel(int idTablero, List<Tablero> tableros, List<Tarea> tareas, List<Usuario> usuarios)
        {
            IdTablero = idTablero;
            Tableros = tableros;
            Tareas = tareas;
            Usuarios = usuarios;
        }

        public ListarTareasViewModel()
        {
            
        }

        public void Inicializar(List<Tablero> tableros, List<Tarea> tareas)
        {
            Tableros = tableros;
            Tareas = tareas;
        }
        
        public void InicializarTareas(List<Tarea> tareas, List<Usuario> usuarios)
        {
            Tareas = tareas;
            Usuarios = usuarios;
        }
    }
}
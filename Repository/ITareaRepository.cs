using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.Repository
{
    public interface ITareaRepository
    {
        public Tarea Create(int idTablero, Tarea tarea);
        public void Update(int idTarea, Tarea tarea);
        public Tarea Get(int idTarea);
        public List<Tarea> GetAllPorUsuario(int idUsuario);
        public List<Tarea> GetAllPorTablero(int idTablero);
        public void Delete(int idTarea);
        public void AsignarUsuarioATarea(int idUsuario, int idTarea);
        public List<Tarea> GetAllTareas();
    }
}
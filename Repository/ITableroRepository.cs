using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.Repository
{
    public interface ITableroRepository
    {
        public void Create(Tablero tablero);
        public void Update(int idTablero,  Tablero tablero);
        public Tablero Get(int idTablero);
        public List<Tablero> GetAll();
        public List<Tablero> GetAllTableros(int idUsuario);
        public void Delete(int idTablero);
        public Tablero ExisteNombre(string nombre);
    }
}
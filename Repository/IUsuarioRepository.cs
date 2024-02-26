using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.Repository
{
    public interface IUsuarioRepository
    {
        public void CreateUsuario(Usuario usuario);
        public void UpdateUsuario(int idUsuario, Usuario usuario);
        public List<Usuario> GetAllUsuarios();
        public Usuario GetUsuario(int idUsuario);
        public void DeleteUsuario(int idUsuario);
        public Usuario AutenticarUsuario(string nombre, string contrasenia);
        public Usuario ExisteNombre(string nombre);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.ViewModels;

namespace TP10.Models
{
    public enum Roles{
        Administrador,
        Operador
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contrasenia { get; set; }
        public Roles? Rol { get; set; }

        public Usuario(LoginViewModel loginViewModel)
        {
            NombreDeUsuario = loginViewModel.NombreDeUsuario;
            Contrasenia = loginViewModel.Contrasenia;
        }

        public Usuario()
        {
            
        }
    }
}
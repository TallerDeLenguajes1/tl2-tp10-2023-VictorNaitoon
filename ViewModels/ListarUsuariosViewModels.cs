using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.ViewModels
{
    public class ListarUsuariosViewModels
    {
        public List<Usuario> Usuarios { get; set; }
        public bool EsAdministrador { get; set; }

        public ListarUsuariosViewModels()
        {
            
        }

        public ListarUsuariosViewModels(List<Usuario> usuarios, bool esAdmin)
        {
            Usuarios = usuarios;
            EsAdministrador = esAdmin;
        }
    }
}
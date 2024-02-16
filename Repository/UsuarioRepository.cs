using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string _cadenaConexion;

        public UsuarioRepository(string CadenaDeConexion)
        {
            _cadenaConexion = CadenaDeConexion;
        }

        public void CreateUsuario(Usuario usuario)
        {
            var query = $"INSERT INTO usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombre_de_usuario, @contrasenia, @rol)";

            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                var command = new SQLiteCommand(query, conexion);

                /*PARA NO TENER PROBLEMAS CON LAS LETRAS MAYUSCULAS O MINUSCULAS SE ME OCURRIO
                PONER EL TOLOWER() PARA GUARDAR Y CUANDO MANDO PARA AUTENTIFICAR, DESPUES VER BIEN COMO
                ES EL TEMA ESTE*/
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }
        public List<Usuario> GetAllUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            var query = $"SELECT * FROM usuario;";

            using(SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(query, conexion);

                conexion.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.Id = Convert.ToInt32(reader["id_usuario"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Rol = (Roles)Convert.ToInt32(reader["rol"]);
                        usuarios.Add(usuario);
                    }
                }
                conexion.Close();
            }
            return usuarios;
        }
        public Usuario GetUsuario(int idUsuario)
        {
            Usuario usuario = new Usuario();

            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"SELECT * FROM usuario WHERE id_usuario = @idUsuario";

                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario.Id = Convert.ToInt32(reader["id_usuario"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Rol = (Roles)Convert.ToInt32(reader["rol"]);
                    }
                }

                conexion.Close();
            }

            return usuario;
        }
        public void UpdateUsuario(int idUsuario, Usuario usuario)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();

                command.CommandText = $"UPDATE usuario SET nombre_de_usuario = @nombre, contrasenia = @contrasenia, rol = @rol WHERE id_usuario = @idUsuario";

                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                command.Parameters.Add(new SQLiteParameter("@nombre", usuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public void DeleteUsuario(int idUsuario)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();

                command.CommandText = $"DELETE FROM usuario WHERE id_usuario = @idUsuario";

                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public Usuario AutenticarUsuario(string nombre, string contrasenia)
        {
            Usuario usuario = new Usuario();
            using(SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = "SELECT * FROM usuario WHERE nombre_de_usuario COLLATE NOCASE = @nombre OR contrasenia = @contrasenia";

                command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", contrasenia));

                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        usuario.Id = Convert.ToInt32(reader["id_usuario"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Rol = (Roles)Convert.ToInt32(reader["rol"]);
                    }
                }

                conexion.Close();
            }
            return usuario;
        }
    }
}
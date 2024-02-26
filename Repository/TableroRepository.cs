using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.Repository
{
    public class TableroRepository : ITableroRepository
    {
        private string _cadenaConexion;

        public TableroRepository(string CadenaDeConexion)
        {
            _cadenaConexion = CadenaDeConexion;
        }

        public void Create(Tablero tablero)
        {
            var query = $"INSERT INTO tablero (id_usuario_propietario, nombre, descripcion) VALUES (@idUsuarioPropietario, @nombre, @descripcion)";

            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = new SQLiteCommand(query, conexion);

                command.Parameters.Add(new SQLiteParameter("@idUsuarioPropietario", tablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public void Delete(int idTablero)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"DELETE FROM tablero WHERE id_tablero = @idTablero";

                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public Tablero Get(int idTablero)
        {
            Tablero tablero = new Tablero();

            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = "SELECT * FROM tablero WHERE id_tablero = @idTablero";
                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tablero.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        tablero.Nombre = reader["nombre"].ToString();
                        tablero.Descripcion = reader["descripcion"].ToString();
                    }
                }

                conexion.Close();
            }
            return tablero;
        }

        public List<Tablero> GetAll()
        {
            List<Tablero> tableros = new List<Tablero>();
            var query = $"SELECT * FROM tablero";

            using(SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = new SQLiteCommand(query, conexion);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero();
                        tablero.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        tablero.Nombre = reader["nombre"].ToString();
                        tablero.Descripcion = reader["descripcion"].ToString();
                        tableros.Add(tablero);
                    }
                }

                conexion.Close();
            }
            

            return tableros;
        }

        //Este metodo nos devuelve los tableros que pertenezcan al usuario del id que le pasamos
        public List<Tablero> GetAllTableros(int idUsuario)
        {
            List<Tablero> tableros = new List<Tablero>();

            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();

                command.CommandText = $"SELECT * FROM tablero WHERE id_usuario_propietario = @idUsuario";
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero();
                        tablero.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        tablero.Nombre = reader["nombre"].ToString();
                        tablero.Descripcion = reader["descripcion"].ToString();

                        tableros.Add(tablero);
                    }
                }

                conexion.Close();
            }

            return tableros;
        }

        public void Update(int idTablero, Tablero tablero)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();

                command.CommandText = $"UPDATE tablero SET id_usuario_propietario = @idUsuarioPropietario, nombre = @nombre, descripcion = @descripcion WHERE id_tablero = @idTablero";

                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                command.Parameters.Add(new SQLiteParameter("@idUsuarioPropietario", tablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public Tablero ExisteNombre(string nombre)
        {
            Tablero tablero = null;

            using (SQLiteConnection conexion = new SQLiteConnection(_cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"SELECT * FROM tablero WHERE nombre COLLATE NOCASE = @nombre";
                command.Parameters.Add(new SQLiteParameter("@nombre", nombre));

                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        tablero = new Tablero();
                        tablero.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        tablero.Nombre = reader["nombre"].ToString();
                        tablero.Descripcion = reader["descripcion"].ToString();
                    }
                }

                conexion.Close();
            }
            return tablero;
        }
    }
}
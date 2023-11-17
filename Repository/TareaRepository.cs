using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP10.Models;

namespace TP10.Repository
{
    public class TareaRepository : ITareaRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

        public void AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"UPDATE tarea SET id_usuario_asignado = @idUsuarioAsignado WHERE id_tarea = @idTarea";
                command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", idUsuario));
                command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public Tarea Create(int idTablero, Tarea tarea)
        {
            var query = $"INSERT INTO tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) VALUES (@idTablero, @nombre, @estado, @descripcion, @color, @idUsuarioPropietario)";

            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = new SQLiteCommand(query, conexion);

                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
                command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
                command.Parameters.Add(new SQLiteParameter("@idUsuarioPropietario", tarea.IdUsuarioAsignado));

                command.ExecuteNonQuery();

                conexion.Close();
            }

            return tarea;
        }

        public void Delete(int idTarea)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"DELETE FROM tarea WHERE id_tarea = @idTarea";
                command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        public Tarea Get(int idTarea)
        {
            Tarea tarea = new Tarea();

            using(SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"SELECT * FROM tarea WHERE id_tarea = @idTarea";
                command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tarea.IdTarea = Convert.ToInt32(reader["id_tarea"]);
                        tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tarea.Nombre = reader["nombre"].ToString();
                        tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                        tarea.Descripcion = reader["descripcion"].ToString();
                        tarea.Color = reader["color"].ToString();
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    }
                }
                conexion.Close();
            }

            return tarea;
        }



        public List<Tarea> GetAllPorTablero(int idTablero)
        {
            List<Tarea> tareas = new List<Tarea>();

            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"SELECT * FROM tarea WHERE id_tablero = @idTablero";
                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Tarea tarea = new Tarea();
                        tarea.IdTarea = Convert.ToInt32(reader["id_tarea"]);
                        tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tarea.Nombre = reader["nombre"].ToString();
                        tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                        tarea.Descripcion = reader["descripcion"].ToString();
                        tarea.Color = reader["color"].ToString();
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                        tareas.Add(tarea);
                    }
                }

                conexion.Close();
            }

            return tareas;
        }




        public List<Tarea> GetAllPorUsuario(int idUsuario)
        {
            List<Tarea> tareas = new List<Tarea>();

            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"SELECT * FROM tarea WHERE id_tablero = @idUsuario";
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea();
                        tarea.IdTarea = Convert.ToInt32(reader["id_tarea"]);
                        tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tarea.Nombre = reader["nombre"].ToString();
                        tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                        tarea.Descripcion = reader["descripcion"].ToString();
                        tarea.Color = reader["color"].ToString();
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                        tareas.Add(tarea);
                    }
                }

                conexion.Close();
            }

            return tareas;
        }

        public void Update(int idTarea, Tarea tarea)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"UPDATE tarea SET id_tablero = @idTablero, nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color, id_usuario_asignado = @idUsuarioAsignado WHERE id_tarea = @idTarea";

                command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                command.Parameters.Add(new SQLiteParameter("@idTablero", tarea.IdTablero));
                command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
                command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
                command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tarea.IdUsuarioAsignado));

                command.ExecuteNonQuery();

                conexion.Close();
            }
        }

        //Implemento un nuevo metodo para poder listar todas las tareas sin importar a que tablero y que usuario pertenezca o sea asignado
        public List<Tarea> GetAllTareas()
        {
            List<Tarea> tareas = new List<Tarea>();

            using (SQLiteConnection conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = $"SELECT * FROM tarea";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea();
                        tarea.IdTarea = Convert.ToInt32(reader["id_tarea"]);
                        tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tarea.Nombre = reader["nombre"].ToString();
                        tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                        tarea.Descripcion = reader["descripcion"].ToString();
                        tarea.Color = reader["color"].ToString();
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                        tareas.Add(tarea);
                    }
                }

                conexion.Close();
            }

            return tareas;
        }
    }
}
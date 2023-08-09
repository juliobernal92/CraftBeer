using CraftBeer.Entidades;
using CraftBeer.Utilidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Dao
{
    public class RecetasDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<Recetas> GetRecetas()
        {
            List<Recetas> listaRecetas = new List<Recetas>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdReceta, Estilo, Litros FROM Recetas";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Recetas receta = new Recetas();
                receta.IdReceta = dataReader.GetInt32(0);
                receta.Estilo = dataReader.GetString(1);
                receta.Litros = dataReader.GetDecimal(2);

                listaRecetas.Add(receta);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaRecetas;
        }

        public bool InsertarReceta(Recetas receta)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Recetas (Estilo, Litros) VALUES (@estilo, @litros)";
            try
            {
                Conexion conexion = new Conexion();

                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@estilo", receta.Estilo);
                comando.Parameters.AddWithValue("@litros", receta.Litros);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al insertar: " + ex.Message);
            }
            return filasAfectadas > 0;
        }

        public bool ActualizarReceta(Recetas receta)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Recetas SET Estilo = @Estilo, Litros = @Litros WHERE IdReceta = @IdReceta";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Estilo", receta.Estilo);
                comando.Parameters.AddWithValue("@Litros", receta.Litros);
                comando.Parameters.AddWithValue("@IdReceta", receta.IdReceta);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al actualizar: " + ex.Message);
            }
            return filasAfectadas > 0;
        }

        public bool EliminarReceta(int idReceta)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM Recetas WHERE IdReceta = @idReceta";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@idReceta", idReceta);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al eliminar: " + ex.Message);
            }
            return filasAfectadas > 0;
        }

        public Dictionary<string, object> ObtenerFilaPorID(long idRecetas)
        {
            Dictionary<string, object> fila = new Dictionary<string, object>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT * FROM Recetas WHERE IdReceta = @IdReceta";
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@IdReceta", idRecetas);

            dataReader = comando.ExecuteReader();
            comando.Parameters.Clear();
            if (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    string columna = dataReader.GetName(i);
                    object valor = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);
                    fila.Add(columna, valor);
                }
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return fila;
        }


        public List<Recetas> GetRecetasPorEstilo(string estilo)
        {
            List<Recetas> listaRecetas = new List<Recetas>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdReceta, Estilo, Litros FROM Recetas WHERE Estilo = @Estilo";
            comando.CommandType = CommandType.Text;

            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@Estilo", estilo);

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Recetas receta = new Recetas();
                receta.IdReceta = dataReader.GetInt32(0);
                receta.Estilo = dataReader.GetString(1);
                receta.Litros = dataReader.GetDecimal(2);

                listaRecetas.Add(receta);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaRecetas;
        }



    }

}

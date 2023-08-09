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
    public class RecetaInsumoDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<RecetaInsumo> GetRecetaInsumos()
        {
            List<RecetaInsumo> listaRecetaInsumos = new List<RecetaInsumo>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdRecetaInsumo, IdReceta, IdInsumo, Kilos FROM RecetaInsumo";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                RecetaInsumo recetaInsumo = new RecetaInsumo();
                recetaInsumo.IdRecetaInsumo = dataReader.GetInt32(0);
                recetaInsumo.IdReceta = dataReader.GetInt32(1);
                recetaInsumo.IdInsumo = dataReader.GetInt32(2);
                recetaInsumo.Kilos = dataReader.GetDecimal(3);

                listaRecetaInsumos.Add(recetaInsumo);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaRecetaInsumos;
        }

        public bool InsertarRecetaInsumo(RecetaInsumo recetaInsumo)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO RecetaInsumo (IdReceta, IdInsumo, Kilos) VALUES (@idReceta, @idInsumo, @kilos)";
            try
            {
                Conexion conexion = new Conexion();

                using (SqlCommand comando = new SqlCommand(sql, conexion.AbrirConexion()))
                {
                    comando.CommandType = CommandType.Text;

                    comando.Parameters.AddWithValue("@idReceta", recetaInsumo.IdReceta);
                    comando.Parameters.AddWithValue("@idInsumo", recetaInsumo.IdInsumo);
                    comando.Parameters.AddWithValue("@kilos", recetaInsumo.Kilos);

                    filasAfectadas = comando.ExecuteNonQuery();
                }

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al insertar: " + ex.Message);
            }

            return filasAfectadas > 0;
        }


        public bool ActualizarRecetaInsumo(RecetaInsumo recetaInsumo)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE RecetaInsumo SET IdReceta = @idReceta, IdInsumo = @idInsumo, Kilos = @kilos WHERE IdRecetaInsumo = @idRecetaInsumo";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@idReceta", recetaInsumo.IdReceta);
                comando.Parameters.AddWithValue("@idInsumo", recetaInsumo.IdInsumo);
                comando.Parameters.AddWithValue("@kilos", recetaInsumo.Kilos);
                comando.Parameters.AddWithValue("@idRecetaInsumo", recetaInsumo.IdRecetaInsumo);

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

        public bool EliminarRecetaInsumo(int idRecetaInsumo)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM RecetaInsumo WHERE IdRecetaInsumo = @idRecetaInsumo";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@idRecetaInsumo", idRecetaInsumo);

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

        public List<RecetaInsumo> ObtenerRecetaInsumoPorIdReceta(int idReceta)
        {
            List<RecetaInsumo> listaRecetaInsumos = new List<RecetaInsumo>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdRecetaInsumo, IdReceta, IdInsumo, Kilos FROM RecetaInsumo WHERE IdReceta = @IdReceta";
            comando.CommandType = CommandType.Text;

            // Agregar parámetro de IdReceta
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IdReceta", idReceta);

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                RecetaInsumo recetaInsumo = new RecetaInsumo();
                recetaInsumo.IdRecetaInsumo = dataReader.GetInt32(0);
                recetaInsumo.IdReceta = dataReader.GetInt32(1);
                recetaInsumo.IdInsumo = dataReader.GetInt32(2);
                recetaInsumo.Kilos = dataReader.GetDecimal(3);

                listaRecetaInsumos.Add(recetaInsumo);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaRecetaInsumos;
        }




    }

}

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
    public class StockInsumosDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<StockInsumos> GetStockInsumos()
        {
            List<StockInsumos> listaStockInsumos = new List<StockInsumos>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdInsumo, TipoInsumo, Nombre, Kilos, CostoKilos FROM StockInsumos";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                StockInsumos stockInsumos = new StockInsumos();
                stockInsumos.IdInsumo = dataReader.GetInt32(0);
                stockInsumos.TipoInsumo = dataReader.GetString(1);
                stockInsumos.Nombre = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                stockInsumos.Kilos = dataReader.GetDecimal(3);
                stockInsumos.CostoKilos = dataReader.GetDecimal(4);

                listaStockInsumos.Add(stockInsumos);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaStockInsumos;
        }

        public bool InsertarStockInsumo(StockInsumos stockInsumos)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO StockInsumos (TipoInsumo, Nombre, Kilos, CostoKilos) VALUES (@tipoInsumo, @nombre, @kilos, @costoKilos)";
            try
            {
                Conexion conexion = new Conexion();

                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@tipoInsumo", stockInsumos.TipoInsumo);
                comando.Parameters.AddWithValue("@nombre", (object)stockInsumos.Nombre ?? DBNull.Value);
                comando.Parameters.AddWithValue("@kilos", stockInsumos.Kilos);
                comando.Parameters.AddWithValue("@costoKilos", stockInsumos.CostoKilos);

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

        public bool ActualizarStockInsumo(StockInsumos stockInsumos)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE StockInsumos SET TipoInsumo = @tipoInsumo, Nombre = @nombre, Kilos = @kilos, CostoKilos = @costoKilos WHERE IdInsumo = @idInsumo";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@tipoInsumo", stockInsumos.TipoInsumo);
                comando.Parameters.AddWithValue("@nombre", (object)stockInsumos.Nombre ?? DBNull.Value);
                comando.Parameters.AddWithValue("@kilos", stockInsumos.Kilos);
                comando.Parameters.AddWithValue("@costoKilos", stockInsumos.CostoKilos);
                comando.Parameters.AddWithValue("@idInsumo", stockInsumos.IdInsumo);

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

        public bool EliminarStockInsumo(int idInsumo)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM StockInsumos WHERE IdInsumo = @idInsumo";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@idInsumo", idInsumo);

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
    }

}

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
    public class StockProductoDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<StockProducto> GetStockProductos()
        {
            List<StockProducto> listaStockProductos = new List<StockProducto>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdEstilo, Estilo, Cantidad, Descripcion, IdLote FROM StockProducto";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                StockProducto stockProducto = new StockProducto();
                stockProducto.IdEstilo = dataReader.GetInt32(0);
                stockProducto.Estilo = dataReader.GetString(1);
                stockProducto.Cantidad = dataReader.GetInt32(2);
                stockProducto.Descripcion = dataReader.GetString(3);
                stockProducto.IdLote = dataReader.GetInt32(4);

                listaStockProductos.Add(stockProducto);
            }

            dataReader.Close();
            conexion.CerrarConexion();

            return listaStockProductos;
        }

        public bool InsertarStockProducto(StockProducto stockProducto)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO StockProducto (Estilo, Cantidad, Descripcion, IdLote) VALUES (@Estilo, @Cantidad, @Descripcion, @IdLote)";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Estilo", stockProducto.Estilo);
                comando.Parameters.AddWithValue("@Cantidad", stockProducto.Cantidad);
                comando.Parameters.AddWithValue("@Descripcion", stockProducto.Descripcion);
                comando.Parameters.AddWithValue("@IdLote", stockProducto.IdLote);

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

        public bool ActualizarStockProducto(StockProducto stockProducto)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE StockProducto SET Estilo = @Estilo, Cantidad = @Cantidad, Descripcion = @Descripcion, IdLote = @IdLote WHERE IdEstilo = @IdEstiloActualizar";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Estilo", stockProducto.Estilo);
                comando.Parameters.AddWithValue("@Cantidad", stockProducto.Cantidad);
                comando.Parameters.AddWithValue("@IdEstiloActualizar", stockProducto.IdEstilo);
                comando.Parameters.AddWithValue("@Descripcion", stockProducto.Descripcion);
                comando.Parameters.AddWithValue("@IdLote", stockProducto.IdLote);

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


        public bool EliminarStockProducto(int idEstilo)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM StockProducto WHERE IdEstilo = @IdEstilo";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@IdEstilo", idEstilo);

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

        public bool EstiloExiste(string nombreEstilo)
        {
            bool existe = false;
            string sql = "SELECT COUNT(*) FROM StockProducto WHERE Estilo = @estilo";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;
                comando.Parameters.AddWithValue("@estilo", nombreEstilo);
                
                int count = Convert.ToInt32(comando.ExecuteScalar());

                existe = count > 0;

                comando.Parameters.Clear();
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar si el estilo existe: " + ex.Message);
            }

            return existe;
        }

        public StockProducto ObtenerStockProductoPorId(int idEstilo)
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdEstilo, Estilo, Cantidad, Descripcion, IdLote FROM StockProducto WHERE IdEstilo = @idEstilo";
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@idEstilo", idEstilo);

            dataReader = comando.ExecuteReader();

            StockProducto stockProducto = null;

            if (dataReader.Read())
            {
                stockProducto = new StockProducto();
                stockProducto.IdEstilo = dataReader.GetInt32(0);
                stockProducto.Estilo = dataReader.GetString(1);
                stockProducto.Cantidad = dataReader.GetInt32(2);
                stockProducto.Descripcion = dataReader.GetString(3);
                stockProducto.IdLote = dataReader.GetInt32(4);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            comando.Parameters.Clear(); // Limpiar los parámetros antes de la siguiente consulta

            return stockProducto;
        }


        public bool RestarProductos(int idStockProducto, int cantidadRestar)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE StockProducto SET Cantidad = Cantidad - @CantidadRestar WHERE IdEstilo = @IdEstilo";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@CantidadRestar", cantidadRestar);
                comando.Parameters.AddWithValue("@IdEstilo", idStockProducto);

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


        public List<StockProducto> GetUltimosTresStockProductos()
        {
            List<StockProducto> listaStockProductos = new List<StockProducto>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT TOP 3 IdEstilo, Estilo, Cantidad, Descripcion, IdLote FROM StockProducto ORDER BY IdEstilo DESC";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                StockProducto stockProducto = new StockProducto();
                stockProducto.IdEstilo = dataReader.GetInt32(0);
                stockProducto.Estilo = dataReader.GetString(1);
                stockProducto.Cantidad = dataReader.GetInt32(2);
                stockProducto.Descripcion = dataReader.GetString(3);
                stockProducto.IdLote = dataReader.GetInt32(4);

                listaStockProductos.Add(stockProducto);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return listaStockProductos;
        }

        public List<StockProducto> GetStockProductosPorIds(List<int> idEstilos)
        {
            List<StockProducto> listaStockProductos = new List<StockProducto>();
            comando.Connection = conexion.AbrirConexion();

            // Crear el comando SQL con parámetros para cada IdEstilo en la lista
            var idsParametros = string.Join(",", idEstilos.Select((id, index) => $"@idEstilo{index}"));
            comando.CommandText = $"SELECT IdEstilo, Estilo, Cantidad, Descripcion, IdLote FROM StockProducto WHERE IdEstilo IN ({idsParametros})";
            comando.CommandType = CommandType.Text;

            // Agregar los parámetros con sus respectivos valores
            for (int i = 0; i < idEstilos.Count; i++)
            {
                comando.Parameters.AddWithValue($"@idEstilo{i}", idEstilos[i]);
            }

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                StockProducto stockProducto = new StockProducto();
                stockProducto.IdEstilo = dataReader.GetInt32(0);
                stockProducto.Estilo = dataReader.GetString(1);
                stockProducto.Cantidad = dataReader.GetInt32(2);
                stockProducto.Descripcion = dataReader.GetString(3);
                stockProducto.IdLote = dataReader.GetInt32(4);

                listaStockProductos.Add(stockProducto);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            comando.Parameters.Clear(); // Limpiar los parámetros antes de la siguiente consulta

            return listaStockProductos;
        }



    }

}

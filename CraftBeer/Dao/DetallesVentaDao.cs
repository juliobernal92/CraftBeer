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
    public class DetallesVentaDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<DetallesVenta> GetDetallesVenta()
        {
            List<DetallesVenta> listaDetallesVenta = new List<DetallesVenta>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdDetalle, IdVenta, IdEstilo, Cantidad, PrecioUnitario, Valor FROM DetallesVenta";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                DetallesVenta detallesVenta = new DetallesVenta();
                detallesVenta.IdDetalle = dataReader.GetInt32(0);
                detallesVenta.IdVenta = dataReader.GetInt32(1);
                detallesVenta.IdEstilo = dataReader.GetInt32(2);
                detallesVenta.Cantidad = dataReader.GetInt32(3);
                detallesVenta.PrecioUnitario = dataReader.GetInt32(4);
                detallesVenta.Valor = dataReader.GetInt32(5);

                listaDetallesVenta.Add(detallesVenta);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return listaDetallesVenta;
        }

        public bool InsertarDetalleVenta(DetallesVenta detalleVenta)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO DetallesVenta (IdVenta, IdEstilo, Cantidad, PrecioUnitario, Valor) VALUES (@IdVenta, @IdEstilo, @Cantidad, @PrecioUnitario, @Valor)";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@IdVenta", detalleVenta.IdVenta);
                comando.Parameters.AddWithValue("@IdEstilo", detalleVenta.IdEstilo);
                comando.Parameters.AddWithValue("@Cantidad", detalleVenta.Cantidad);
                comando.Parameters.AddWithValue("@PrecioUnitario", detalleVenta.PrecioUnitario);
                comando.Parameters.AddWithValue("@Valor", detalleVenta.Valor);

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

        public bool EliminarDetalleVenta(int idDetalle)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM DetallesVenta WHERE IdDetalle = @IdDetalle";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@IdDetalle", idDetalle);

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

        public bool ActualizarDetalleVenta(DetallesVenta detalleVenta)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE DetallesVenta SET IdVenta = @IdVenta, IdEstilo = @IdEstilo, Cantidad = @Cantidad, PrecioUnitario = @PrecioUnitario, Valor = @Valor WHERE IdDetalle = @IdDetalle";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@IdVenta", detalleVenta.IdVenta);
                comando.Parameters.AddWithValue("@IdEstilo", detalleVenta.IdEstilo);
                comando.Parameters.AddWithValue("@Cantidad", detalleVenta.Cantidad);
                comando.Parameters.AddWithValue("@PrecioUnitario", detalleVenta.PrecioUnitario);
                comando.Parameters.AddWithValue("@Valor", detalleVenta.Valor);
                comando.Parameters.AddWithValue("@IdDetalle", detalleVenta.IdDetalle);

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

        public List<DetallesVenta> GetDetallesVentaPorIdVenta(int idVenta)
        {
            List<DetallesVenta> listaDetallesVenta = new List<DetallesVenta>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdDetalle, IdVenta, IdEstilo, Cantidad, PrecioUnitario, Valor FROM DetallesVenta WHERE IdVenta = @idVenta";
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@idVenta", idVenta);

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                DetallesVenta detallesVenta = new DetallesVenta();
                detallesVenta.IdDetalle = dataReader.GetInt32(0);
                detallesVenta.IdVenta = dataReader.GetInt32(1);
                detallesVenta.IdEstilo = dataReader.GetInt32(2);
                detallesVenta.Cantidad = dataReader.GetInt32(3);
                detallesVenta.PrecioUnitario = dataReader.GetInt32(4);
                detallesVenta.Valor = dataReader.GetInt32(5);

                listaDetallesVenta.Add(detallesVenta);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return listaDetallesVenta;
        }



        

        

    }

}

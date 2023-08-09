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
    public class VentaDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<Venta> GetVentas()
        {
            List<Venta> listaVentas = new List<Venta>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdVenta, Fecha, IdCliente, TotalIva, MontoTotal FROM Ventas";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Venta venta = new Venta();
                venta.IdVenta = dataReader.GetInt32(0);
                venta.Fecha = dataReader.GetDateTime(1);
                venta.IdCliente = dataReader.GetInt32(2);
                if (!dataReader.IsDBNull(3))
                    venta.TotalIva = dataReader.GetInt32(3);
                if (!dataReader.IsDBNull(4))
                    venta.MontoTotal = dataReader.GetInt32(4);

                listaVentas.Add(venta);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return listaVentas;
        }

        public bool InsertarVenta(Venta venta)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Ventas (Fecha, IdCliente, TotalIva, MontoTotal) VALUES (@Fecha, @IdCliente, @TotalIva, @MontoTotal)";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Fecha", venta.Fecha);
                comando.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                comando.Parameters.AddWithValue("@TotalIva", (object)venta.TotalIva ?? DBNull.Value);
                comando.Parameters.AddWithValue("@MontoTotal", (object)venta.MontoTotal ?? DBNull.Value);



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

        public bool EliminarVenta(int idVenta)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM Ventas WHERE IdVenta = @IdVenta";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@IdVenta", idVenta);

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

        public bool ActualizarVenta(Venta venta)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Ventas SET Fecha = @Fecha, IdCliente = @IdCliente, TotalIva = @TotalIva, MontoTotal = @MontoTotal WHERE IdVenta = @IdVenta";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Fecha", venta.Fecha);
                comando.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                comando.Parameters.AddWithValue("@TotalIva", venta.TotalIva);
                comando.Parameters.AddWithValue("@MontoTotal", venta.MontoTotal);
                comando.Parameters.AddWithValue("@IdVenta", venta.IdVenta);

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


        public bool Insert(Venta venta, out int idVenta)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Ventas (Fecha, IdCliente, TotalIva, MontoTotal) VALUES (@Fecha, @IdCliente, @TotalIva, @MontoTotal); SELECT SCOPE_IDENTITY();";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Fecha", venta.Fecha);
                comando.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                comando.Parameters.AddWithValue("@TotalIva", (object)venta.TotalIva ?? DBNull.Value);
                comando.Parameters.AddWithValue("@MontoTotal", (object)venta.MontoTotal ?? DBNull.Value);

                object result = comando.ExecuteScalar();
                idVenta = Convert.ToInt32(result);

                filasAfectadas = 1;
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al insertar: " + ex.Message);
            }

            return filasAfectadas > 0;
        }


        public bool ActualizarMontoTotalYTotalIva(Venta venta)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Ventas SET MontoTotal = @MontoTotal, TotalIva = @TotalIva WHERE IdVenta = @IdVenta";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@MontoTotal", venta.MontoTotal);
                comando.Parameters.AddWithValue("@TotalIva", venta.TotalIva);
                comando.Parameters.AddWithValue("@IdVenta", venta.IdVenta);

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

        public List<Venta> GetUltimasTresVentas()
        {
            List<Venta> listaVentas = new List<Venta>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT TOP 3 IdVenta, Fecha, IdCliente, TotalIva, MontoTotal FROM Ventas ORDER BY Fecha DESC";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Venta venta = new Venta();
                venta.IdVenta = dataReader.GetInt32(0);
                venta.Fecha = dataReader.GetDateTime(1);
                venta.IdCliente = dataReader.GetInt32(2);
                if (!dataReader.IsDBNull(3))
                    venta.TotalIva = dataReader.GetInt32(3);
                if (!dataReader.IsDBNull(4))
                    venta.MontoTotal = dataReader.GetInt32(4);

                listaVentas.Add(venta);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return listaVentas;
        }

        public List<Venta> GetVentaById(int idVenta)
        {
            List<Venta> listaVentas = new List<Venta>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdVenta, Fecha, IdCliente, TotalIva, MontoTotal FROM Ventas WHERE IdVenta = @IdVenta";
            comando.CommandType = CommandType.Text;
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IdVenta", idVenta);

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Venta venta = new Venta();
                venta.IdVenta = dataReader.GetInt32(0);
                venta.Fecha = dataReader.GetDateTime(1);
                venta.IdCliente = dataReader.GetInt32(2);
                if (!dataReader.IsDBNull(3))
                    venta.TotalIva = dataReader.GetInt32(3);
                if (!dataReader.IsDBNull(4))
                    venta.MontoTotal = dataReader.GetInt32(4);

                listaVentas.Add(venta);
            }

            dataReader.Close();
            conexion.CerrarConexion();
            return listaVentas;
        }

    }

}

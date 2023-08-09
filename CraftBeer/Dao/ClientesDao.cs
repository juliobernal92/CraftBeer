using CraftBeer.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraftBeer.Utilidades;

namespace CraftBeer.Dao
{
    public class ClientesDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<Clientes> GetClientes()
        {
            List<Clientes> listaClientes = new List<Clientes>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdCliente, Nombre, RUC, Telefono, Direccion FROM Clientes";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Clientes cliente = new Clientes();
                cliente.IdCliente = dataReader.GetInt32(0);
                cliente.Nombre = dataReader.GetString(1);
                cliente.RUC = dataReader.GetString(2);
                cliente.Telefono = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                cliente.Direccion = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);

                listaClientes.Add(cliente);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaClientes;
        }

        public bool InsertarCliente(Clientes cliente)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Clientes (Nombre, RUC, Telefono, Direccion) VALUES (@nombre, @ruc, @telefono, @direccion)";
            try
            {
                Conexion conexion = new Conexion();

                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                comando.Parameters.AddWithValue("@ruc", cliente.RUC);
                comando.Parameters.AddWithValue("@telefono", (object)cliente.Telefono ?? DBNull.Value);
                comando.Parameters.AddWithValue("@direccion", (object)cliente.Direccion ?? DBNull.Value);

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

        public bool ActualizarCliente(Clientes cliente)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Clientes SET Nombre = @nombre, RUC = @ruc, Telefono = @telefono, Direccion = @direccion WHERE IdCliente = @idCliente";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                comando.Parameters.AddWithValue("@ruc", cliente.RUC);
                comando.Parameters.AddWithValue("@telefono", (object)cliente.Telefono ?? DBNull.Value);
                comando.Parameters.AddWithValue("@direccion", (object)cliente.Direccion ?? DBNull.Value);
                comando.Parameters.AddWithValue("@idCliente", cliente.IdCliente);

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

        public bool EliminarCliente(int idCliente)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM Clientes WHERE IdCliente = @idCliente";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@idCliente", idCliente);

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


        public bool Insert(Clientes cliente, out int idCliente)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Clientes (RUC, Nombre, Direccion, Telefono) VALUES (@RUC, @Nombre, @Direccion, @Telefono); SELECT SCOPE_IDENTITY();";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@RUC", cliente.RUC);
                comando.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                comando.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                comando.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                object result = comando.ExecuteScalar();
                idCliente = Convert.ToInt32(result);

                filasAfectadas = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al insertar el cliente: " + ex.Message);
            }
            return (filasAfectadas > 0);
        }


        public Clientes GetClienteById(int idCliente)
        {
            Clientes cliente = null;
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdCliente, Nombre, RUC, Telefono, Direccion FROM Clientes WHERE IdCliente = @IdCliente";
            comando.CommandType = CommandType.Text;

            // Limpiar los parámetros existentes
            comando.Parameters.Clear();

            // Agregar el nuevo parámetro
            comando.Parameters.AddWithValue("@IdCliente", idCliente);

            dataReader = comando.ExecuteReader();

            if (dataReader.Read())
            {
                cliente = new Clientes();
                cliente.IdCliente = dataReader.GetInt32(0);
                cliente.Nombre = dataReader.GetString(1);
                cliente.RUC = dataReader.GetString(2);
                cliente.Telefono = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                cliente.Direccion = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
            }

            dataReader.Close();
            conexion.CerrarConexion();

            return cliente;
        }


        public Clientes GetClienteByRuc(string ruc)
        {
            Clientes cliente = null;
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdCliente, Nombre, RUC, Telefono, Direccion FROM Clientes WHERE RUC = @RUC";
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@RUC", ruc);

            dataReader = comando.ExecuteReader();

            if (dataReader.Read())
            {
                cliente = new Clientes();
                cliente.IdCliente = dataReader.GetInt32(0);
                cliente.Nombre = dataReader.GetString(1);
                cliente.RUC = dataReader.GetString(2);
                cliente.Telefono = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                cliente.Direccion = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
            }

            dataReader.Close();
            conexion.CerrarConexion();

            comando.Parameters.Clear(); // Limpiar los parámetros después de cada consulta

            return cliente;
        }


    }

}

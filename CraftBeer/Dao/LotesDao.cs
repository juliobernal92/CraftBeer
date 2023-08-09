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
    public class LotesDao
    {
        private Conexion conexion = new Conexion();
        private SqlDataReader dataReader;
        private SqlCommand comando = new SqlCommand();

        public List<Lotes> GetLotes()
        {
            List<Lotes> listaLotes = new List<Lotes>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdLote, Maestro, Estilo, FechaInicio, DensidadInicial, Estado, DensidadFinal, FechaFin, LitrosFinales, ABV FROM Lotes";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Lotes lote = new Lotes();
                lote.IdLote = dataReader.GetInt32(0);
                lote.Maestro = dataReader.GetString(1);
                lote.Estilo = dataReader.GetString(2);
                lote.FechaInicio = dataReader.GetDateTime(3);
                lote.DensidadInicial = dataReader.GetDecimal(4);
                lote.Estado = dataReader.GetString(5);
                lote.DensidadFinal = dataReader.GetDecimal(6);
                lote.FechaFin = dataReader.GetDateTime(7);
                lote.LitrosFinales = dataReader.GetDecimal(8);
                lote.ABV = dataReader.GetDecimal(9);

                listaLotes.Add(lote);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaLotes;
        }

        public bool InsertarLote(Lotes lote)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Lotes (Maestro, Estilo, FechaInicio, DensidadInicial, Estado, DensidadFinal, FechaFin, LitrosFinales, ABV) VALUES (@maestro, @estilo, @fechaInicio, @densidadInicial, @estado, @densidadFinal, @fechaFin, @litrosFinales, @abv)";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@maestro", lote.Maestro);
                comando.Parameters.AddWithValue("@estilo", lote.Estilo);
                comando.Parameters.Add("@fechaInicio", SqlDbType.Date).Value = lote.FechaInicio;
                comando.Parameters.Add(new SqlParameter("@densidadInicial", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = lote.DensidadInicial
                });
                comando.Parameters.AddWithValue("@estado", lote.Estado);
                comando.Parameters.Add(new SqlParameter("@densidadFinal", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = lote.DensidadFinal
                });

                comando.Parameters.Add("@fechaFin", SqlDbType.Date).Value = lote.FechaFin;
                comando.Parameters.AddWithValue("@abv", lote.ABV);
                comando.Parameters.AddWithValue("@litrosFinales", lote.LitrosFinales);

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

        public bool ActualizarLote(Lotes lote)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Lotes SET Maestro = @maestro, Estilo = @estilo, FechaInicio = @fechaInicio, DensidadInicial = @densidadInicial, Estado = @estado, DensidadFinal = @densidadFinal, FechaFin = @fechaFin, LitrosFinales = @litrosFinales, ABV = @abv WHERE IdLote = @idLote";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@maestro", lote.Maestro);
                comando.Parameters.AddWithValue("@estilo", lote.Estilo);
                comando.Parameters.Add("@fechaInicio", SqlDbType.Date).Value = lote.FechaInicio;
                comando.Parameters.Add(new SqlParameter("@densidadInicial", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = lote.DensidadInicial
                });
                comando.Parameters.AddWithValue("@estado", lote.Estado);
                comando.Parameters.Add(new SqlParameter("@densidadFinal", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 3,
                    Value = lote.DensidadFinal
                });
                comando.Parameters.Add("@fechaFin", SqlDbType.Date).Value = lote.FechaFin;
                comando.Parameters.Add(new SqlParameter("@abv", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 1,
                    Value = lote.ABV
                });
                comando.Parameters.AddWithValue("@litrosFinales", lote.LitrosFinales);
                comando.Parameters.AddWithValue("@idLote", lote.IdLote);

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

        public bool EliminarLote(int idLote)
        {
            int filasAfectadas = 0;
            string sql = "DELETE FROM Lotes WHERE IdLote = @idLote";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@idLote", idLote);

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


        public bool RestarLitros(Lotes lote)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE lotes SET LitrosFinales = @litrosFinales WHERE Idlote = @idLote";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@litrosFinales", lote.Resta);
                comando.Parameters.AddWithValue("@idLote", lote.IdLote);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return filasAfectadas > 0;
        }


        public List<Lotes> GetUltimosTresLotes()
        {
            List<Lotes> listaLotes = new List<Lotes>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT TOP 3 IdLote, Maestro, Estilo, FechaInicio, DensidadInicial, Estado, DensidadFinal, FechaFin, LitrosFinales, ABV FROM Lotes ORDER BY IdLote DESC";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                Lotes lote = new Lotes();
                lote.IdLote = dataReader.GetInt32(0);
                lote.Maestro = dataReader.GetString(1);
                lote.Estilo = dataReader.GetString(2);
                lote.FechaInicio = dataReader.GetDateTime(3);
                lote.DensidadInicial = dataReader.GetDecimal(4);
                lote.Estado = dataReader.GetString(5);
                lote.DensidadFinal = dataReader.GetDecimal(6);
                lote.FechaFin = dataReader.GetDateTime(7);
                lote.LitrosFinales = dataReader.GetDecimal(8);
                lote.ABV = dataReader.GetDecimal(9);

                listaLotes.Add(lote);
            }
            dataReader.Close();
            conexion.CerrarConexion();
            return listaLotes;
        }


    }

}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CraftBeer.Utilidades
{
    public class Conexion
    {
        private SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexionSQL_SERVER"].ConnectionString);

        public SqlConnection AbrirConexion()
        {
            if (sqlConexion.State == ConnectionState.Closed)
                sqlConexion.Open();
            return sqlConexion;
        }
        public SqlConnection CerrarConexion()
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
            return sqlConexion;
        }
    }
}

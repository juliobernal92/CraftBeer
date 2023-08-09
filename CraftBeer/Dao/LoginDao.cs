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
    public class LoginDao
    {
        private Conexion conexion = new Conexion(); //objeto conexion
        private SqlDataReader dataReader; // declarar la variable
        private SqlCommand comando = new SqlCommand();

        public List<Login> GetUser()
        {
            List<Login> listaUsuarios = new List<Login>();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT IdUsuario, Nombres, Apellidos, Email, Usuario, Contraseña FROM Login";
            comando.CommandType = CommandType.Text;

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())//true
            {
                //crear objeto entidad Stock
                Login log = new Login();
                log.Id_User = dataReader.GetInt32(0);
                log.Nombre = dataReader.GetString(1);
                log.Apellido = dataReader.GetString(2);
                log.Email = dataReader.GetString(3);
                log.Usuario = dataReader.GetString(4);
                log.Password = dataReader.GetString(5);

                listaUsuarios.Add(log);
            }
            //liberar recursos de la memoria
            dataReader.Close();
            conexion.CerrarConexion();
            // retornar lista stock
            return listaUsuarios;
        }

        public bool Insertar(Login login)
        {
            int filasAfectadas = 0;
            string sql = "INSERT INTO Login (Nombres, Apellidos, Email, Usuario, Contraseña) VALUES (@Nombres, @Apellidos, @Email, @Usuario, @Contraseña)";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Nombres", login.Nombre);
                comando.Parameters.AddWithValue("@Apellidos", login.Apellido);
                comando.Parameters.AddWithValue("@Email", login.Email);
                comando.Parameters.AddWithValue("@Usuario", login.Usuario);
                comando.Parameters.AddWithValue("@Contraseña", login.Password);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();

            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al insertar: " + ex.Message);
            }
            return (filasAfectadas > 0) ? true : false;
        }

        public bool ActualizarSinPass(Login login)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Login SET Nombres = @nombres, Apellidos = @apellidos, Email = @email WHERE IdUsuario=@idUsuario";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@nombres", login.Nombre);
                comando.Parameters.AddWithValue("@apellidos", login.Apellido);
                comando.Parameters.AddWithValue("@email", login.Email);
                comando.Parameters.AddWithValue("idUsuario", login.Id_User);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al actualizar" + ex.Message);
            }
            return (filasAfectadas > 0) ? true : false;
        }

        public bool ActualizarTodo(Login login)
        {
            int filasAfectadas = 0;
            string sql = "UPDATE Login SET Nombres = @nombres, Apellidos = @apellidos, Email = @email, Contraseña = @contraseña WHERE IdUsuario=@idUsuario";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@nombres", login.Nombre);
                comando.Parameters.AddWithValue("@apellidos", login.Apellido);
                comando.Parameters.AddWithValue("@email", login.Email);
                comando.Parameters.AddWithValue("@contraseña", login.Password);
                comando.Parameters.AddWithValue("idUsuario", login.Id_User);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al actualizar" + ex.Message);
            }
            return (filasAfectadas > 0) ? true : false;
        }

        public bool ActualizarContraseña(Login login)
        {
            int filasAfectadas = 0;
            string sql = "update Login set Contraseña=@Contraseña where Usuario=@Usuario";

            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@Contraseña", login.Password);
                comando.Parameters.AddWithValue("@Usuario", login.Usuario);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al actualizar" + ex.Message);
            }
            return (filasAfectadas > 0) ? true : false;
        }

        public bool EliminarUsuario(long idUser)
        {
            int filasAfectadas = 0;
            string sql = "delete from Login where IdUsuario=@IdUsuario";
            try
            {
                comando.Connection = conexion.AbrirConexion();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;

                comando.Parameters.AddWithValue("@IdUsuario", idUser);

                filasAfectadas = comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error al eliminar: " + ex.Message);
            }
            return (filasAfectadas > 0) ? true : false;
        }



        public int GetIdUsuarioByUsuario(string usuario)
        {
            int idUsuario = -1; // Valor por defecto si no se encuentra el usuario

            List<Login> listaUsuarios = GetUser();

            // Buscar el usuario en la lista
            foreach (Login usuarioActual in listaUsuarios)
            {
                if (usuarioActual.Usuario == usuario)
                {
                    idUsuario = usuarioActual.Id_User;
                    break; // Salir del bucle una vez que se encuentra el usuario
                }
            }

            return idUsuario;
        }


        public Login GetUsuarioById(int idUsuario)
        {
            Login usuario = null;

            List<Login> listaUsuarios = GetUser();

            // Buscar el usuario por IdUsuario
            foreach (Login usuarioActual in listaUsuarios)
            {
                if (usuarioActual.Id_User == idUsuario)
                {
                    usuario = usuarioActual;
                    break;
                }
            }

            return usuario;
        }



    }
}

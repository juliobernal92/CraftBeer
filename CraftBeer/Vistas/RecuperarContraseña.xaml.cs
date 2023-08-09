using CraftBeer.Dao;
using CraftBeer.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CraftBeer.Vistas
{
    /// <summary>
    /// Lógica de interacción para RecuperarContraseña.xaml
    /// </summary>
    public partial class RecuperarContraseña : Window
    {
        public RecuperarContraseña()
        {
            InitializeComponent();
        }

        private void btnRecuperar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtusuario.Text;
            string nuevaContraseña = txtpassword.Password;

            // Validar si los campos están vacíos
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(nuevaContraseña))
            {
                MessageBox.Show("Por favor, ingrese el usuario y la nueva contraseña.");
                return;
            }

            // Verificar si el usuario existe en la base de datos
            LoginDao loginDao = new LoginDao();
            List<Login> usuarios = loginDao.GetUser();
            Login usuarioExistente = usuarios.FirstOrDefault(u => u.Usuario == usuario);

            if (usuarioExistente == null)
            {
                MessageBox.Show("El usuario no existe. Por favor, verifique el nombre de usuario.");
                return;
            }

            // Actualizar la contraseña del usuario
            usuarioExistente.Password = nuevaContraseña;
            bool exito = loginDao.ActualizarContraseña(usuarioExistente);

            if (exito)
            {
                MessageBox.Show("Contraseña actualizada exitosamente.");
                // Aquí puedes realizar alguna acción adicional, como cerrar la ventana actual o mostrar un mensaje adicional.
            }
            else
            {
                MessageBox.Show("Error al actualizar la contraseña. Por favor, inténtelo nuevamente.");
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }
    }
}

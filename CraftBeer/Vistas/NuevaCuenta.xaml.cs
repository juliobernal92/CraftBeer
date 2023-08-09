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
    /// Lógica de interacción para NuevaCuenta.xaml
    /// </summary>
    public partial class NuevaCuenta : Window
    {
        public NuevaCuenta()
        {
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtnombre.Text;
            string apellido = txtapellido.Text;
            string email = txtemail.Text;
            string usuario = txtusuario.Text;
            string password = txtpassword.Password;

            // Validar si los campos están vacíos
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            // Verificar si el usuario ya existe en la base de datos
            LoginDao loginDao = new LoginDao();
            List<Login> usuarios = loginDao.GetUser();
            bool usuarioExistente = usuarios.Exists(u => u.Usuario == usuario);

            if (usuarioExistente)
            {
                MessageBox.Show("El usuario ya existe. Por favor, elija otro nombre de usuario.");
                return;
            }

            // Crear una instancia del objeto Login con los datos ingresados
            Login nuevoUsuario = new Login(/*0, nombre, apellido, email, usuario, password*/)
            {
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Usuario = usuario,
                Password = password
            };

            // Insertar el nuevo usuario en la base de datos
            bool exito = loginDao.Insertar(nuevoUsuario);

            if (exito)
            {
                MessageBox.Show("Usuario creado exitosamente.");
                // Aquí puedes realizar alguna acción adicional, como abrir la ventana Dashboard
            }
            else
            {
                MessageBox.Show("Error al crear el usuario. Por favor, inténtelo nuevamente.");
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

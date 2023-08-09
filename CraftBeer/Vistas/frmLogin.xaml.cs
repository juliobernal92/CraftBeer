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
    /// Lógica de interacción para frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window
    {
        NuevaCuenta crearventana = null;
        RecuperarContraseña recuperarventana = null;
        public frmLogin()
        {
            InitializeComponent();
            txtuser.Focus();
        }

        private void RecuperarContraseña_Click(object sender, RoutedEventArgs e)
        {
            if (recuperarventana == null || !recuperarventana.IsActive)
            {
                recuperarventana = new RecuperarContraseña();
                recuperarventana.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                recuperarventana.Show();
            }
            else
            {
                recuperarventana.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                recuperarventana.Focus();
            }
        }

        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            if (crearventana == null || !crearventana.IsActive)
            {
                crearventana = new NuevaCuenta();
                crearventana.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                crearventana.Show();
            }
            else
            {
                crearventana.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                crearventana.Focus();
            }
        }

        private void btningresar_Click(object sender, RoutedEventArgs e)
        {
            if (txtuser.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre de usuario!", "Cerceceria Yka'a");
                //lblinfo.Text = "El Campo NO Puede Quedar Vacio!";
                txtuser.Focus();
                return;
            }
            if (txtpass.Password == "")
            {
                MessageBox.Show("Debe ingresar la Contraseña", "Cerveceria Yka'a");
                //lblinfo.Text = "El Campo NO Puede Quedar Vacio!";
                txtpass.Focus();
                return;
            }
            else
            {
                /*
                string nombreusuario = txtnombre.Text;
                Dashboard dashok = new Dashboard();
                dashok.NombreUsuario = nombreusuario;
                System.Diagnostics.Debug.WriteLine("Nombre de usuario: " + dashok.NombreUsuario);
                dashok.Show();
                this.Close();
                */

                string usuario = txtuser.Text;
                string contraseña = txtpass.Password;

                // Verificar las credenciales en la base de datos
                bool esValido = VerificarCredenciales(usuario, contraseña);

                if (esValido)
                {
                    // Abrir la ventana del dashboard
                    Dashboard ventanaDashboard = new Dashboard();
                    string user = usuario;
                    ventanaDashboard.getUser(user);
                    ventanaDashboard.Show();

                    // Cerrar la ventana de inicio de sesión
                    Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Inicio de sesión");
                }

            }
        }
        private bool VerificarCredenciales(string usuario, string contraseña)
        {
            LoginDao loginDao = new LoginDao();
            List<Login> usuarios = loginDao.GetUser();

            foreach (Login usuarioBD in usuarios)
            {
                if (usuarioBD.Usuario == usuario && usuarioBD.Password == contraseña)
                {
                    return true;
                }
            }

            return false;
        }

        private void gridbarra_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}

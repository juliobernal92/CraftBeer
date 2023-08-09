using CraftBeer.Dao;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CraftBeer.Entidades;

namespace CraftBeer.Paginas
{
    /// <summary>
    /// Lógica de interacción para pageConfiguracion.xaml
    /// </summary>
    public partial class pageConfiguracion : Page
    {
        LoginDao loginDao = new LoginDao();
        Login login = new Login();
        public pageConfiguracion()
        {
            InitializeComponent();
        }

        public void getUser(string user)
        {
            txtuser.Text = user;
            int iduser = loginDao.GetIdUsuarioByUsuario(user);
            txtidusuario.Text = iduser.ToString();
            Login login = loginDao.GetUsuarioById(iduser);
            txtnombre.Text = login.Nombre;
            txtapellido.Text = login.Apellido;
            txtemail.Text = login.Email;
            
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtnombre.Text.Trim();
            string apellido = txtapellido.Text.Trim();
            string email = txtemail.Text.Trim();
            string currentpass = txtpassactual.Password;
            string newpass1 = txtpassnew1.Password;
            string newpass2 = txtpassnew2.Password;
            string iduser = txtidusuario.Text.Trim();

            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apellido) && !string.IsNullOrEmpty(email)
                 && !string.IsNullOrEmpty(iduser)
                && (string.IsNullOrEmpty(currentpass) || string.IsNullOrEmpty(newpass1) || string.IsNullOrEmpty(newpass2)))
            {
                try
                {
                    Login login = new Login();
                    login.Nombre = nombre;
                    login.Apellido = apellido;
                    login.Email = email;
                    login.Id_User = int.Parse(iduser);

                    if (loginDao.ActualizarSinPass(login))
                    {
                        MessageBox.Show("Datos Actualizados", "CraftBeer Manager");
                        txtnombre.Focus();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apellido) && !string.IsNullOrEmpty(email)
                     && !string.IsNullOrEmpty(iduser)
                    && !string.IsNullOrEmpty(currentpass) && !string.IsNullOrEmpty(newpass1) && !string.IsNullOrEmpty(newpass2))
            {
                try
                {
                    Login login = new Login();
                    login.Nombre = nombre;
                    login.Apellido = apellido;
                    login.Email = email;
                    login.Id_User = int.Parse(iduser);
                    Login usuarios = loginDao.GetUsuarioById(int.Parse(iduser));

                    if (usuarios.Password == currentpass)
                    {
                        if (newpass1 == newpass2)
                        {
                            login.Password = newpass1;
                        }
                        else
                        {
                            MessageBox.Show("La nueva contraseña no coincide", "CraftBeer Manager");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("La contraseña actual no coincide", "CraftBeer Manager");
                        return;
                    }

                    if (loginDao.ActualizarTodo(login))
                    {
                        MessageBox.Show("Datos y Contraseña Actualizadas", "CraftBeer Manager");
                        txtpassactual.Clear();
                        txtpassnew1.Clear();
                        txtpassnew2.Clear();
                        txtnombre.Focus();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }





    }
}

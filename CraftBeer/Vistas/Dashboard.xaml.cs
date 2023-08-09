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
using CraftBeer.Paginas;

namespace CraftBeer.Vistas
{
    /// <summary>
    /// Lógica de interacción para Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private pageDashboard dash;
        private DateTime _lastClickTime = DateTime.Now;
        private const int _doubleClickDelay = 500;
        private Button selectedButton;
        public Dashboard()
        {
            InitializeComponent();
            selectedButton = null;
            txtid.Loaded += Txtid_Loaded;
        }

        private void Txtid_Loaded(object sender, RoutedEventArgs e)
        {
            // El valor de txtid.Text estará disponible aquí, ya que el control se ha cargado.
            string user = txtid.Text;

            // Crear la instancia de pageDashboard y establecer el usuario
            dash = new pageDashboard();
            dash.getUser(user);
            framePrincipal.Content = dash;
        }

        public void getUser(string user)
        {
            txtid.Text = user;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void stockInsumos_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Content = new pageStockInsumos();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            pageDashboard dash = new pageDashboard();
            string user = txtid.Text;
            dash.getUser(user);
            framePrincipal.Content = dash;
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnLote_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Content = new pageLote();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnInventarioProducto_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Content = new pageStockProductos();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Content = new pageVentas();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnGastos_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Content = new pageGastos();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            pageConfiguracion config = new pageConfiguracion();
            string user = txtid.Text;
            config.getUser(user);

            framePrincipal.Content = config;
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            frmLogin logoff = new frmLogin();
            logoff.Show();
            this.Close();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnReceta_Click(object sender, RoutedEventArgs e)
        {
            framePrincipal.Content = new pageAñadirReceta();
            if (selectedButton != null)
            {
                selectedButton.Foreground = Brushes.Black;
            }
            ((Button)sender).Foreground = Brushes.Brown;
            selectedButton = (Button)sender;
        }

        private void btnUtil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gridbarra_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - _lastClickTime;

            if (elapsedTime.TotalMilliseconds <= _doubleClickDelay)
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else this.WindowState = WindowState.Normal;
            }

            _lastClickTime = currentTime;
        }
    }
}

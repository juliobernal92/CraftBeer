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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CraftBeer.Paginas
{
    /// <summary>
    /// Lógica de interacción para pageInventarioProducto.xaml
    /// </summary>
    public partial class pageInventarioProducto : Page
    {
        LotesDao loteDao;
        StockProductoDao stockProductoDao;
        List<Lotes> listaLote = new List<Lotes>();
        List<StockProducto> listaStock = new List<StockProducto>();
        Dictionary<int, Lotes> diccionarioLotes = new Dictionary<int, Lotes>();


        public pageInventarioProducto()
        {
            InitializeComponent();
            ConfigInicial();
            CargarDatos();
        }

        private void ConfigInicial()
        {
            loteDao = new LotesDao();
            stockProductoDao = new StockProductoDao();
            rdbtnBotellas.Checked += rdbtnBotellas_Checked;
            rdbtnBarriles.Checked += rdbtnBarriles_Checked;

        }

        private void CargarDatos()
        {
            LotesDao loteDao = new LotesDao();
            List<Lotes> listaLotes = loteDao.GetLotes();
            cbestilos.ItemsSource = listaLotes;
            listaStock = stockProductoDao.GetStockProductos();
            dgStock.ItemsSource = listaStock;
            diccionarioLotes = listaLote.ToDictionary(lote => lote.IdLote);
        }

        private void cbestilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbestilos.SelectedItem != null)
            {
                Lotes loteSeleccionado = cbestilos.SelectedItem as Lotes;
                txtidlote.Text = loteSeleccionado.IdLote.ToString();
                txtlitrosdisponibles.Text = loteSeleccionado.LitrosFinales.ToString();
            }
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbestilos.Text) || rdbtnBarriles.IsChecked == false && rdbtnBotellas.IsChecked == false)
            {
                MessageBox.Show("Por favor, completa todos los campos", "CraftBeer Manager");
                return;
            }
            StockProducto stockProductos = new StockProducto();

            if (rdbtnBotellas.IsChecked == true)
            {
                txtlitrosbarril.IsEnabled = false;
                string litrosDisponiblesText = txtlitrosdisponibles.Text;

                if (int.TryParse(litrosDisponiblesText, out int litrosDisponibles))
                {
                    float botellas = litrosDisponibles / 0.5f;
                    float cantidad = float.Parse(txtcantidad.Text);
                    if (cantidad <= botellas)
                    {
                        stockProductos.Cantidad = int.Parse(txtcantidad.Text);

                        if (cbestilos.SelectedItem != null)
                        {
                            Lotes selectedItem = (Lotes)cbestilos.SelectedItem;
                            stockProductos.Estilo = selectedItem.Estilo + " - Botellas";
                        }
                    }
                    else
                    {
                        MessageBox.Show("El valor ingresado supera la cantidad de litros disponible en el lote seleccionado", "CraftBeer Manager");
                        return;
                    }
                }
                else
                {
                    lblreferenciabotellas.Content = "Seleccione un Estilo";
                    return;
                }
                txtlitrosbarril.IsEnabled = false;
            }
            else
            {
                txtlitrosbarril.IsEnabled = true;
                string litrosdisp = txtlitrosdisponibles.Text;
                float litrosdispfloat = float.Parse(litrosdisp);
                string litrosacargar = txtlitrosbarril.Text;
                float litrosacargarfloat = float.Parse(litrosacargar);
                float litrototal = litrosacargarfloat + litrosacargarfloat;
                if (litrototal <= litrosdispfloat)
                {
                    stockProductos.Cantidad = int.Parse(txtcantidad.Text);
                    if (cbestilos.SelectedItem != null)
                    {
                        Lotes selectedItem = (Lotes)cbestilos.SelectedItem;
                        stockProductos.Estilo = selectedItem.Estilo + " - Barril";
                    }
                }
                else
                {
                    MessageBox.Show("Los litros ingresados superan a la cantidad del lote", "CraftBeer Manager");
                    return;
                }
            }

            StockProductoDao stockProductoDao = new StockProductoDao();
            StockProducto stockExistente = stockProductoDao.GetStockProductos().FirstOrDefault(s => s.Estilo == stockProductos.Estilo);


            if (stockExistente != null)
            {
                // El estilo ya existe, se actualizan los datos
                stockExistente.Cantidad = stockProductos.Cantidad;

                if (stockProductoDao.ActualizarStockProducto(stockExistente))
                {
                    MessageBox.Show("El estilo se actualizó exitosamente.", "CraftBeer Manager");
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el estilo.", "CraftBeer Manager");
                }
            }
            else
            {
                // El estilo no existe, se crea un nuevo registro
                if (stockProductoDao.InsertarStockProducto(stockProductos))
                {
                    MessageBox.Show("Nuevo estilo añadido exitosamente.", "CraftBeer Manager");
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Error al guardar el nuevo estilo.", "CraftBeer Manager");
                }
            }

            txtidStock.Clear();
            txtidlote.Clear();
            txtcantidad.Clear();
            txtlitrosbarril.Clear();
            txtlitrosdisponibles.Clear();
            cbestilos.SelectedItem = null;
        }



        private void rdbtnBotellas_Checked(object sender, RoutedEventArgs e)
        {
            txtlitrosbarril.IsEnabled = false;
            string litrosDisponiblesText = txtlitrosdisponibles.Text;

            if (int.TryParse(litrosDisponiblesText, out int litrosDisponibles))
            {
                float botellas = litrosDisponibles / 0.5f;
                string labeltext = "Cantidad de botellas: " + botellas;
                lblreferenciabotellas.Content = labeltext;
            }
            else
            {
                lblreferenciabotellas.Content = "Seleccione un Estilo";
            }

        }

        private void rdbtnBarriles_Checked(object sender, RoutedEventArgs e)
        {
            txtlitrosbarril.IsEnabled = true;

            string litrosDisponiblesText = txtlitrosdisponibles.Text;

            if (int.TryParse(litrosDisponiblesText, out int litrosDisponibles))
            {
                float barriles = litrosDisponibles / 20f;
                string labeltext = "Cantidad de barriles: " + barriles;
                lblreferenciabotellas.Content = labeltext;
            }
            else
            {
                lblreferenciabotellas.Content = "Seleccione un Estilo";
            }
        }



        private void btnEliminarStock_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Estas seguro?", "CraftBeer Manager", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Int32 IdStock = Convert.ToInt32(((Button)sender).Tag);

                if (stockProductoDao.EliminarStockProducto(IdStock))
                {
                    MessageBox.Show("Eliminado!", "CraftBeer Manager",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation
                    );
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("No se pudo Eliminar", "CraftBeer Manager",
                       MessageBoxButton.OK, MessageBoxImage.Error
                   );
                }
            }
        }

        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

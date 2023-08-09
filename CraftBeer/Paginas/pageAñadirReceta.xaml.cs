using CraftBeer.Dao;
using CraftBeer.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para pageAñadirReceta.xaml
    /// </summary>
    public partial class pageAñadirReceta : Page
    {
        RecetasDao recetasDao = new RecetasDao();
        RecetaInsumoDao recetainsumoDao = new RecetaInsumoDao();
        StockInsumosDao stockInsumosDao = new StockInsumosDao();
        List<Recetas> listaRecetas = new List<Recetas>();
        List<RecetaInsumo> listaRecetaInsumo = new List<RecetaInsumo>();
        List<StockInsumos> listaStockInsumos = new List<StockInsumos>();
        String estiloSeleccionado;
        public pageAñadirReceta()
        {
            InitializeComponent();
            ConfigInicial();
            CargarDatos();
        }
        private void ConfigInicial()
        {
            recetasDao = new RecetasDao();
            recetainsumoDao = new RecetaInsumoDao();
            stockInsumosDao = new StockInsumosDao();
            txtestilo.Focus();

        }
        private void CargarDatos()
        {
            listaRecetas = recetasDao.GetRecetas();
            dgreceta.ItemsSource = listaRecetas;

            listaStockInsumos = stockInsumosDao.GetStockInsumos();
            

            cbrecetas.ItemsSource = listaRecetas.Select(e => e.Estilo).ToList();
            cbtipoinsumo.ItemsSource = listaStockInsumos.Select(e => e.TipoInsumo).ToList();
            
        }


        private void btnañadir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtestilo.Text) || string.IsNullOrEmpty(txtlitros.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos", "CraftBeer Manager");
                return;
            }

            try
            {
                Recetas recetas = new Recetas();
                recetas.Estilo = txtestilo.Text;
                recetas.Litros = int.Parse(txtlitros.Text);
                List<Recetas> estilos = recetasDao.GetRecetas();
                if (string.IsNullOrEmpty(txtidreceta.Text))
                {
                    
                    bool estiloExistente = estilos.Exists(u => u.Estilo == recetas.Estilo);
                    if (estiloExistente)
                    {
                        MessageBox.Show("Este estilo ya existe, ingrese otro", "CraftBeer Manager");
                        txtidreceta.Text = string.Empty;
                        txtestilo.Text = string.Empty;
                        txtlitros.Text = string.Empty;
                        txtestilo.Focus();
                        return;
                    }

                    Recetas nuevaReceta = new Recetas()
                    {
                        Estilo = recetas.Estilo,
                        Litros = recetas.Litros
                    };

                    bool ok = recetasDao.InsertarReceta(nuevaReceta);

                    if (ok)
                    {
                        MessageBox.Show("Receta Creada Correctamente", "CraftBeer Manager");
                        CargarDatos();
                        txtidreceta.Clear();
                        txtestilo.Clear();
                        txtlitros.Clear();
                        txtestilo.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Error al crear la receta. Por favor, inténtelo nuevamente.");
                    }
                }
                else
                {
                    
                    recetas.IdReceta = Convert.ToInt32(txtidreceta.Text);


                    if (recetasDao.ActualizarReceta(recetas))
                    {
                        MessageBox.Show("Actualizado exitosamente!", "CraftBeer Manager");
                        CargarDatos();
                        txtidreceta.Clear();
                        txtestilo.Clear();
                        txtlitros.Clear();
                        txtestilo.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar", "CraftBeer Manager");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnseleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (cbrecetas.SelectedItem == null)
            {
                MessageBox.Show("No se ha seleccionado ningún estilo.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            string estiloSeleccionado = cbrecetas.SelectedItem.ToString();

            Recetas recetaSeleccionada = listaRecetas.FirstOrDefault(r => r.Estilo == estiloSeleccionado);

            if (recetaSeleccionada == null)
            {
                MessageBox.Show("No se encontró la receta seleccionada.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            txtidreceta.Text = recetaSeleccionada.IdReceta.ToString();
            txtestilo.Text = recetaSeleccionada.Estilo;
            txtlitros.Text = recetaSeleccionada.Litros.ToString();

            // Obtener los datos de RecetaInsumo y StockInsumos asociados a la receta seleccionada
            List<RecetaInsumo> listaRecetaInsumos = recetainsumoDao.ObtenerRecetaInsumoPorIdReceta(recetaSeleccionada.IdReceta);
            var data = from recetaInsumo in listaRecetaInsumos
                       join stockInsumo in listaStockInsumos on recetaInsumo.IdInsumo equals stockInsumo.IdInsumo
                       select new
                       {
                           recetaInsumo.IdRecetaInsumo,
                           recetaInsumo.IdInsumo,
                           recetaInsumo.Kilos,
                           stockInsumo.TipoInsumo,
                           stockInsumo.Nombre
                       };

            dgrecetainsumo.ItemsSource = data;
        }

        private void btnañadirinsumo_Click(object sender, RoutedEventArgs e)
        {
            if (cbrecetas.SelectedItem == null)
            {
                MessageBox.Show("No se ha seleccionado ningún estilo.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (cbtipoinsumo.SelectedItem == null && cbnombreinsumo.SelectedItem == null && string.IsNullOrEmpty(txtcantidad.Text))
            {
                MessageBox.Show("Por favor, seleccione todos los campos.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string nombreSeleccionado = cbnombreinsumo.SelectedItem as string;
            StockInsumos insumoSeleccionado = listaStockInsumos.FirstOrDefault(r => r.Nombre == nombreSeleccionado);
            if (insumoSeleccionado == null)
            {
                MessageBox.Show("No se encontró el insumo seleccionado.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RecetaInsumo recetaInsumo = new RecetaInsumo();
            recetaInsumo.IdReceta = int.Parse(txtidreceta.Text);
            recetaInsumo.IdInsumo = insumoSeleccionado.IdInsumo;
            recetaInsumo.Kilos = decimal.Parse(txtcantidad.Text);
            if (recetainsumoDao.InsertarRecetaInsumo(recetaInsumo))
            {
                MessageBox.Show("Insumo Añadido Correctamente", "CraftBeer Manager");
                ActualizarDataGridRecetaInsumo();
                cbtipoinsumo.SelectedItem = null;
                cbnombreinsumo.SelectedItem = null;
                txtcantidad.Clear();
            }
            else
            {
                MessageBox.Show("Error al Añadir Insumo", "CraftBeer Manager");
            }
        }

        private void ActualizarDataGridRecetaInsumo()
        {
            if (cbrecetas.SelectedItem == null)
            {
                MessageBox.Show("No se ha seleccionado ningún estilo.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string estiloSeleccionado = cbrecetas.SelectedItem.ToString();

            Recetas recetaSeleccionada = listaRecetas.FirstOrDefault(r => r.Estilo == estiloSeleccionado);

            if (recetaSeleccionada == null)
            {
                MessageBox.Show("No se encontró la receta seleccionada.", "CraftBeer Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int idRecetaSeleccionada = recetaSeleccionada.IdReceta;

            // Obtener los datos de RecetaInsumo por IdReceta
            List<RecetaInsumo> listaRecetaInsumos = recetainsumoDao.ObtenerRecetaInsumoPorIdReceta(idRecetaSeleccionada);

            // Filtrar los resultados por IdReceta
            var data = from recetaInsumo in listaRecetaInsumos
                       join stockInsumo in listaStockInsumos on recetaInsumo.IdInsumo equals stockInsumo.IdInsumo
                       where recetaInsumo.IdReceta == idRecetaSeleccionada
                       select new
                       {
                           recetaInsumo.IdRecetaInsumo,
                           recetaInsumo.IdInsumo,
                           recetaInsumo.Kilos,
                           stockInsumo.TipoInsumo,
                           stockInsumo.Nombre
                       };

            // Asignar la lista filtrada como origen de datos del DataGrid
            dgrecetainsumo.ItemsSource = data;
        }


        private void btnEditarReceta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 IdReceta = Convert.ToInt32(((Button)sender).Tag);
                Recetas recetas;

                recetas = listaRecetas.Where(a => a.IdReceta == IdReceta).FirstOrDefault();

                this.txtidreceta.Text = recetas.IdReceta.ToString();
                this.txtestilo.Text = recetas.Estilo.ToString();
                this.txtlitros.Text = recetas.Litros.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }

        private void btnEliminarReceta_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Estas seguro?", "CraftBeer Manager", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Int32 IdReceta = Convert.ToInt32(((Button)sender).Tag);

                if (recetasDao.EliminarReceta(IdReceta))
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

        private void btnEditarRecetaInsumo_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnEliminarRecetaInsumo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Estas seguro?", "CraftBeer Manager", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Int32 IdRecetaInsumo = Convert.ToInt32(((Button)sender).Tag);

                if (recetainsumoDao.EliminarRecetaInsumo(IdRecetaInsumo))
                {
                    MessageBox.Show("Eliminado!", "CraftBeer Manager",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation
                    );
                    CargarDatos();
                    ActualizarDataGridRecetaInsumo();
                }
                else
                {
                    MessageBox.Show("No se pudo Eliminar", "CraftBeer Manager",
                       MessageBoxButton.OK, MessageBoxImage.Error
                   );
                }
            }
        }

        private void cbtipoinsumo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tipoInsumoSeleccionado = cbtipoinsumo.SelectedItem as string;
            List<StockInsumos> stockInsumosFiltrados = listaStockInsumos.Where(s => s.TipoInsumo == tipoInsumoSeleccionado).ToList();
            cbnombreinsumo.ItemsSource = stockInsumosFiltrados.Select(s => s.Nombre).ToList();
        }

        private void btnlimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtidreceta.Clear();
            txtestilo.Clear();
            txtlitros.Clear();
        }
    }
}

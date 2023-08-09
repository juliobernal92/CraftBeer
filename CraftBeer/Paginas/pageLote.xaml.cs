using CraftBeer.Dao;
using CraftBeer.Entidades;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// Lógica de interacción para pageLote.xaml
    /// </summary>
    public partial class pageLote : Page
    {
        RecetasDao recetasDao;
        LotesDao loteDao;
        List<Recetas> listaRecetas = new List<Recetas>();
        List<Lotes> listaLote = new List<Lotes>();

        DateTime solofechafin;
        public pageLote()
        {
            InitializeComponent();
            ConfigInicial();
            CargarDatos();
        }
        private void ConfigInicial()
        {
            recetasDao = new RecetasDao();
            loteDao = new LotesDao();

            if (string.IsNullOrEmpty(txtidlote.Text))
            {
                //cbEstado.IsEnabled = false;
                txtDF.IsEnabled = false;
                txtFechaFin.IsEnabled = false;
                txtlitrosfinal.IsEnabled = false;
            }
            else
            {
                //cbEstado.IsEnabled = true;
                txtDF.IsEnabled = true;
                txtFechaFin.IsEnabled = true;
                txtlitrosfinal.IsEnabled = true;
            }
            DateTime fechaActual = DateTime.Now;
            DateTime soloFecha = fechaActual.Date;
            txtfechainicio.Text = soloFecha.ToString("dd/MM/yyyy");
        }

        private void CargarDatos()
        {
            listaRecetas = recetasDao.GetRecetas();
            listaLote = loteDao.GetLotes();

            cbestilo.ItemsSource = listaRecetas.Select(e => e.Estilo).ToList();
            dgMisLotes.ItemsSource = listaLote;

        }

        
        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtcervecero.Text) || string.IsNullOrEmpty(cbestilo.Text) || string.IsNullOrEmpty(txtDI.Text) || string.IsNullOrEmpty(cbEstado.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos", "CraftBeer Manager");
                return;
            }

            Lotes lote = new Lotes();
            lote.Maestro = txtcervecero.Text;
            lote.Estilo = cbestilo.Text;
            lote.DensidadInicial = decimal.Parse(txtDI.Text);
            // Ingresar Fecha actual
            DateTime fechaActual = DateTime.Today;
            DateTime soloFecha = fechaActual.Date;
            txtfechainicio.Text = soloFecha.ToString("dd/MM/yyyy");
            lote.FechaInicio = DateTime.Parse(txtfechainicio.Text);
            //Ingresar CB ESTADO
            if (cbEstado.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cbEstado.SelectedItem;
                lote.Estado = selectedItem.Content.ToString();
            }

            if (!string.IsNullOrEmpty(txtidlote.Text))
            {
                if (string.IsNullOrEmpty(txtDF.Text))
                {
                    MessageBox.Show("Debes ingresar la densidad final", "CraftBeer Manager");
                    txtDF.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtlitrosfinal.Text))
                {
                    MessageBox.Show("Debes ingresar la cantidad de litros finales", "CraftBeer Manager");
                    txtlitrosfinal.Focus();
                    return;
                }

                lote.DensidadFinal = decimal.Parse(txtDF.Text);

                // Ingresar Fecha Fin
                string fechaString = txtFechaFin.Text;
                DateTime fechafin = DateTime.ParseExact(fechaString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                lote.FechaFin = fechafin;

                // ABV
                decimal di = decimal.Parse(txtDI.Text);
                decimal df = decimal.Parse(txtDF.Text);
                double cons = 131.25;
                decimal constante = (decimal)cons;
                decimal abv = (di - df) * constante;
                lote.ABV = abv;
                lote.LitrosFinales = decimal.Parse(txtlitrosfinal.Text);


            }





            if (string.IsNullOrEmpty(txtidlote.Text))
            {
                if (loteDao.InsertarLote(lote))
                {
                    MessageBox.Show("Lote Añadido exitosamente!", "CraftBeer Manager");
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Error al guardar", "CraftBeer Manager");
                }
            }
            else
            {
                lote.IdLote = Convert.ToInt32(txtidlote.Text);

                if (loteDao.ActualizarLote(lote))
                {
                    MessageBox.Show("Actualizado exitosamente!", "CraftBeer Manager");
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar", "CraftBeer Manager");
                }
            }
            txtidlote.Clear();
            txtcervecero.Clear();
            cbestilo.SelectedItem = null;
            txtDI.Clear();
            cbEstado.SelectedItem = null;
            txtDF.Clear();
            txtFechaFin.Clear();
            txtlitrosfinal.Clear();
            txtcervecero.Focus();
            ConfigInicial();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Path = @"C:\Users\Doomed666\source\repos\CraftBeer\CraftBeer\Reportes\";
                string PathPDF = @"C:\Users\Doomed666\source\repos\CraftBeer\CraftBeer\Pdf\";

                ReportDocument oRep = new ReportDocument();
                oRep.Load(Path + "LoteReporte.rpt");
                CargarDatos();
                oRep.SetDataSource(listaLote);


                oRep.ExportToDisk(ExportFormatType.PortableDocFormat, PathPDF + "LoteReporte.pdf");
                MessageBox.Show("Exito - PDF creado Correctamente");
                Process.Start(PathPDF + "LoteReporte.pdf");

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo exportar: " + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditarDataGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 IdLote = Convert.ToInt32(((Button)sender).Tag);
                Lotes lote;

                lote = listaLote.Where(a => a.IdLote == IdLote).FirstOrDefault();

                this.txtidlote.Text = lote.IdLote.ToString();
                this.txtcervecero.Text = lote.Maestro;
                this.cbestilo.Text = lote.Estilo;
                this.txtfechainicio.Text = lote.FechaInicio.ToString("dd/MM/yyyy");
                this.txtDI.Text = lote.DensidadInicial.ToString();
                this.cbEstado.Text = lote.Estado;
                this.txtDF.Text = lote.DensidadFinal.ToString();
                this.txtFechaFin.Text = lote.FechaFin.ToString("dd/MM/yyyy");
                this.txtlitrosfinal.Text = lote.LitrosFinales.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
            txtDF.IsEnabled = true;
            txtFechaFin.IsEnabled = true;
            txtlitrosfinal.IsEnabled = true;
        }

        private void btnEliminarDataGrid_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Estas seguro?", "CraftBeer Manager", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Int32 IdLote = Convert.ToInt32(((Button)sender).Tag);

                if (loteDao.EliminarLote(IdLote))
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

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtidlote.Clear();
            txtcervecero.Clear();
            cbestilo.SelectedItem = null;
            txtDI.Clear();
            cbEstado.SelectedItem = null;
            txtDF.Clear();
            txtFechaFin.Clear();
            txtlitrosfinal.Clear();
            txtcervecero.Focus();
        }

        
    }
}

using CraftBeer.Dao;
using CraftBeer.Entidades;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Lógica de interacción para pageStockInsumos.xaml
    /// </summary>
    public partial class pageStockInsumos : Page
    {
        StockInsumosDao stockInsumosDao = new StockInsumosDao();
        List<StockInsumos> listaInsumos = new List<StockInsumos>();
        public pageStockInsumos()
        {
            InitializeComponent();
            initConfig();
            cargarDatos();
        }
        private void initConfig()
        {
            stockInsumosDao = new StockInsumosDao();
        }
        private void cargarDatos()
        {
            listaInsumos = stockInsumosDao.GetStockInsumos();
            dginsumos.ItemsSource = listaInsumos;
        }

        private void btnañadir_Click(object sender, RoutedEventArgs e)
        {
            string tipoinsumo = ((ComboBoxItem)cbtipoinsumo.SelectedItem).Content.ToString().Trim();
            string nombre = txtnombre.Text.Trim();
            string kilos = txtcantidad.Text.Trim();
            string costokg = txtcostounitario.Text.Trim();

            if (!string.IsNullOrEmpty(tipoinsumo) && !string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(kilos) && !string.IsNullOrEmpty(costokg))
            {
                try
                {
                    StockInsumos stockinsumos = new StockInsumos();
                    stockinsumos.TipoInsumo = tipoinsumo;
                    stockinsumos.Nombre = nombre;
                    stockinsumos.Kilos = Convert.ToDecimal(kilos);
                    stockinsumos.CostoKilos = Convert.ToDecimal(costokg);

                    if (string.IsNullOrEmpty(txtidinsumo.Text))
                    {
                        if (stockInsumosDao.InsertarStockInsumo(stockinsumos))
                        {
                            MessageBox.Show("Insumo guardado", "CraftBeer Manager");
                            cargarDatos();
                            cbtipoinsumo.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Error al guardar", "CraftBeer Manager");
                        }
                    }
                    else
                    {
                        stockinsumos.IdInsumo = Convert.ToInt32(txtidinsumo.Text);


                        if (stockInsumosDao.ActualizarStockInsumo(stockinsumos))
                        {
                            MessageBox.Show("Actualizado exitosamente!", "CraftBeer Manager");
                            cargarDatos();
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
            else
            {
                MessageBox.Show("Por favor complete todos los campos", "CraftBeer Manager");
            }

            cbtipoinsumo.Focus();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnexportarpdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Path = @"C:\Users\Doomed666\source\repos\CraftBeer\CraftBeer\Reportes\";
                string PathPDF = @"C:\Users\Doomed666\source\repos\CraftBeer\CraftBeer\Pdf\";

                ReportDocument oRep = new ReportDocument();
                oRep.Load(Path + "reporteInsumo.rpt");
                cargarDatos();
                oRep.SetDataSource(listaInsumos);


                oRep.ExportToDisk(ExportFormatType.PortableDocFormat, PathPDF + "InsumosReporte.pdf");
                MessageBox.Show("Exito - PDF creado Correctamente");
                Process.Start(PathPDF + "InsumosReporte.pdf");

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo exportar: " + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditarInsumo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 IdInsumo = Convert.ToInt32(((Button)sender).Tag);
                StockInsumos stkInsumos;

                stkInsumos = listaInsumos.Where(a => a.IdInsumo == IdInsumo).FirstOrDefault();

                this.txtidinsumo.Text = stkInsumos.IdInsumo.ToString();
                this.cbtipoinsumo.Text = stkInsumos.TipoInsumo.ToString();
                this.txtnombre.Text = stkInsumos.Nombre.ToString();
                this.txtcantidad.Text = stkInsumos.Kilos.ToString();
                this.txtcostounitario.Text = stkInsumos.CostoKilos.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }

        private void btnEliminarInsumo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Estas seguro?", "CraftBeer Manager", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Int32 IdInsumo = Convert.ToInt32(((Button)sender).Tag);

                if (stockInsumosDao.EliminarStockInsumo(IdInsumo))
                {
                    MessageBox.Show("Eliminado!", "CraftBeer Manager",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation
                    );
                    cargarDatos();
                }
                else
                {
                    MessageBox.Show("No se pudo Eliminar", "CraftBeer Manager",
                       MessageBoxButton.OK, MessageBoxImage.Error
                   );
                }
            }
        }
    }
}

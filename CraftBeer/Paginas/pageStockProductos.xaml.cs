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
    /// Lógica de interacción para pageStockProductos.xaml
    /// </summary>
    public partial class pageStockProductos : Page
    {
        LotesDao loteDao;
        StockProductoDao stockProductoDao;
        List<Lotes> listaLote = new List<Lotes>();
        List<StockProducto> listaStock = new List<StockProducto>();
        public pageStockProductos()
        {
            InitializeComponent();
            ConfigInicial();
            CargarDatos();
        }
        private void ConfigInicial()
        {
            loteDao = new LotesDao();
            stockProductoDao = new StockProductoDao();
        }
        private void CargarDatos()
        {
            listaLote = loteDao.GetLotes();
            listaStock = stockProductoDao.GetStockProductos();

            cbestilos.ItemsSource = listaLote.Select(e => e.Estilo).ToList();
            dgstockproducto.ItemsSource = listaStock;
        }
        private void cbestilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbestilos.SelectedItem != null)
            {
                string estiloSeleccionado = cbestilos.SelectedItem.ToString();
                Lotes loteSeleccionado = listaLote.FirstOrDefault(l => l.Estilo == estiloSeleccionado);
                if (loteSeleccionado != null)
                {
                    txtlitrosdisp.Text = loteSeleccionado.LitrosFinales.ToString();
                    txtidlote.Text = loteSeleccionado.IdLote.ToString();
                }
            }
        }

        private void btnañadir_Click(object sender, RoutedEventArgs e)
        {
            float cantidadLitrosDisp = float.Parse(txtlitrosdisp.Text);
            if (!string.IsNullOrEmpty(cbestilos.Text) || !string.IsNullOrEmpty(cbpresentacion.Text) || !string.IsNullOrEmpty(txtcantidad.Text))
            {
                string estilos = cbestilos.SelectedItem.ToString().Trim();
                string presentacion = ((ComboBoxItem)cbpresentacion.SelectedItem).Content.ToString().Trim();


                

                StockProducto stockprod = new StockProducto();
                stockprod.Estilo = estilos;
                stockprod.Descripcion = presentacion;
                stockprod.Cantidad = int.Parse(txtcantidad.Text);

                float aRestar = 0.0f;

                string itemSeleccionado = cbpresentacion.SelectedValue.ToString();
                if (itemSeleccionado == "Botellas 500ml")
                {
                    string cantbotstr = txtcantidad.Text;
                    float cantidadBotellas = float.Parse(cantbotstr);
                    float litrosPorBotella = 0.5f;
                    aRestar = cantidadBotellas * litrosPorBotella;
                }
                else if (itemSeleccionado == "Barriles 20L")
                {
                    string cantbarrstr = txtcantidad.Text;
                    float cantidadBarriles = float.Parse(cantbarrstr);
                    float litrosPorBarril = 20.0f;
                    aRestar = cantidadBarriles * litrosPorBarril;
                }
                stockprod.IdLote = int.Parse(txtidlote.Text);


                if (aRestar > cantidadLitrosDisp)
                {
                    MessageBox.Show("La cantidad a restar excede la cantidad disponible de litros", "CraftBeer Manager");
                    return;
                }

                if (string.IsNullOrEmpty(txtidstock.Text))
                {
                    if (stockProductoDao.EstiloExiste(estilos))
                    {
                        MessageBox.Show("Ya existe un estilo con el mismo nombre", "CraftBeer Manager");
                        return;
                    }


                    if (stockProductoDao.InsertarStockProducto(stockprod))
                    {
                        MessageBox.Show("Insertado Correctamente", "CraftBeer Manager");
                        CargarDatos();

                        float cantidadLitros = cantidadLitrosDisp - aRestar;
                        Lotes lote = new Lotes();
                        lote.Resta = cantidadLitros;
                        lote.IdLote = int.Parse(txtidlote.Text);
                        MessageBox.Show("Litros Restantes del Lote: " + cantidadLitros + "  ID: " + int.Parse(txtidlote.Text), "CraftBeer Manager");
                        if (loteDao.RestarLitros(lote))
                        {
                            MessageBox.Show("Litros Restados del Lote", "CraftBeer Manager");
                            CargarDatos();
                            Limpiar();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo restar", "CraftBeer Manager");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al insertar", "CraftBeer Manager");
                    }
                }
                else
                {

                    StockProducto stkactualizar = new StockProducto();
                    stkactualizar.Estilo = estilos;
                    stkactualizar.Descripcion = presentacion;
                    stkactualizar.IdLote = int.Parse(txtidlote.Text);

                    StockProducto cantant = stockProductoDao.ObtenerStockProductoPorId(int.Parse(txtidstock.Text));
                    int cantbd = cantant.Cantidad;
                    int cantsumar = int.Parse(txtcantidad.Text);
                    int cantnew = cantsumar + cantbd;
                    stkactualizar.Cantidad = cantnew;

                    // para restar los litros correctos
                    //botellas
                    float litrdispnow = float.Parse(txtlitrosdisp.Text);
                    float cantnow = float.Parse(txtcantidad.Text);
                    float cantarestarbot = cantnow * 0.5f;

                    float cantTotarestarbot = litrdispnow - cantarestarbot;
                    //barriles
                    float cantarestarbarr = cantnow * 20;
                    float cantTotarestarbarr = litrdispnow - cantarestarbarr;

                    stkactualizar.IdEstilo = Convert.ToInt32(txtidstock.Text);
                    if (stockProductoDao.ActualizarStockProducto(stkactualizar))
                    {
                        MessageBox.Show("Actualizado exitosamente!", "CraftBeer Manager");
                        CargarDatos();

                        StockProducto stockAnterior = stockProductoDao.ObtenerStockProductoPorId(int.Parse(txtidstock.Text));
                        int cantAnterior = stockAnterior.Cantidad;
                        StockProducto desc = stockProductoDao.ObtenerStockProductoPorId(int.Parse(txtidstock.Text));
                        string descrip = desc.Descripcion;
                        Lotes lote = new Lotes();
                        if (descrip == "Botellas 500ml")
                        {
                            lote.Resta = cantTotarestarbot;
                        }
                        else if(descrip=="Barriles 20L")
                        {
                            lote.Resta = cantTotarestarbarr;
                        }
                        lote.IdLote = int.Parse(txtidlote.Text);
                        if (loteDao.RestarLitros(lote))
                        {
                            MessageBox.Show("Litros descontados en lotes", "CraftBeer Manager");
                            CargarDatos();
                            Limpiar();

                        }
                        else
                        {
                            MessageBox.Show("Error al restar", "CraftBeer Manager");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar", "CraftBeer Manager");
                    }
                }


            }
        }


        private void Limpiar()
        {
            txtidlote.Clear();
            txtidstock.Clear();
            txtcantidad.Clear();
            txtlitrosdisp.Clear();
            cbestilos.SelectedItem = null;
            cbpresentacion.SelectedItem = null;
            cbestilos.Focus();
        }
        private void btnlimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void btnexportarPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Path = @"C:\Users\Doomed666\source\repos\CraftBeer\CraftBeer\Reportes\";
                string PathPDF = @"C:\Users\Doomed666\source\repos\CraftBeer\CraftBeer\Pdf\";

                ReportDocument oRep = new ReportDocument();
                oRep.Load(Path + "reporteStock.rpt");
                CargarDatos();
                oRep.SetDataSource(listaStock);


                oRep.ExportToDisk(ExportFormatType.PortableDocFormat, PathPDF + "StockReporte.pdf");
                MessageBox.Show("Exito - PDF creado Correctamente");
                Process.Start(PathPDF + "StockReporte.pdf");

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo exportar: " + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbpresentacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = cbpresentacion.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string mensaje = string.Empty;
                string litrosdisponiblesStr = txtlitrosdisp.Text;

                if (float.TryParse(litrosdisponiblesStr, out float litrosdisp))
                {
                    if (selectedItem.Content.ToString() == "Botellas 500ml")
                    {
                        float botellas = litrosdisp / 0.5f;
                        mensaje = "Información: puedes añadir " + botellas + " botellas";
                    }
                    else if (selectedItem.Content.ToString() == "Barriles 20L")
                    {
                        float barriles = litrosdisp / 20;
                        mensaje = "Información: puedes añadir " + barriles + " barriles";
                    }
                }
                else
                {
                    mensaje = "El valor de litros disponibles no es válido";
                }

                lblinformacion.Content = mensaje;
            }
        }

        private void btnEditarStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 IdInsumo = Convert.ToInt32(((Button)sender).Tag);
                StockProducto stkprod;

                stkprod = listaStock.Where(a => a.IdEstilo == IdInsumo).FirstOrDefault();

                this.txtidstock.Text = stkprod.IdEstilo.ToString();
                this.cbestilos.Text = stkprod.Estilo.ToString();
                this.cbpresentacion.Text = stkprod.Descripcion.ToString();
                this.txtcantidad.Text = stkprod.Cantidad.ToString();
                this.txtidlote.Text = stkprod.IdLote.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "ALERTA",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }

        private void btnEliminarStock_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

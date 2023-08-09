using CraftBeer.Dao;
using CraftBeer.Entidades;
using CraftBeer.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Lógica de interacción para pageVentas.xaml
    /// </summary>
    public partial class pageVentas : Page
    {
        ClientesDao clientesDao = new ClientesDao();
        VentaDao ventaDao = new VentaDao();
        DetallesVentaDao detallesDao = new DetallesVentaDao();
        StockProductoDao stockProductoDao;
        List<DetallesVenta> listaDetalles = new List<DetallesVenta>();
        List<DetallesVenta> listaDetallesReport = new List<DetallesVenta>();
        List<StockProducto> listaStock = new List<StockProducto>();
        List<Venta> listaVentas = new List<Venta>();
        DetallesVenta detallesVnt = new DetallesVenta();

        public pageVentas()
        {
            InitializeComponent();
            ConfigInicial();
            CargarDatos();
        }
        private void ConfigInicial()
        {
            clientesDao = new ClientesDao();
            stockProductoDao = new StockProductoDao();
            detallesDao = new DetallesVentaDao();
            DateTime fechaActual = DateTime.Now;
            DateTime soloFecha = fechaActual.Date;
            txtfecha.Text = soloFecha.ToString("dd/MM/yyyy");
            detallesVnt = new DetallesVenta();
        }
        private void CargarDatos()
        {
            listaStock= stockProductoDao.GetStockProductos();
            listaDetalles = detallesDao.GetDetallesVenta();
            listaVentas = ventaDao.GetVentas();
            cbproductos.ItemsSource = listaStock.Select(e => e.Estilo).ToList();

            
            
        }
        

        private void CargarDataGrid()
        {
            // Obtener los datos de DetallesVenta utilizando DetallesVentaDao
            DetallesVentaDao detallesVentaDao = new DetallesVentaDao();
            List<DetallesVenta> detallesVentas = detallesVentaDao.GetDetallesVenta();

            // Obtener los datos de StockProducto utilizando StockProductoDao
            StockProductoDao stockProductoDao = new StockProductoDao();
            List<StockProducto> stockProductos = stockProductoDao.GetStockProductos();

            // Filtrar los detalles de venta por el IdVenta específico
            int idVenta = int.Parse(txtidventa.Text); // Suponiendo que txtidventa contiene el ID de venta deseado
            detallesVentas = detallesVentas.Where(d => d.IdVenta == idVenta).ToList();

            // Combinar los datos y mostrarlos en el DataGrid
            var dataToShow = from detalleVenta in detallesVentas
                             join stockProducto in stockProductos on detalleVenta.IdEstilo equals stockProducto.IdEstilo
                             select new DetallesVentaConStock
                             {
                                 IdEstilo = detalleVenta.IdEstilo,
                                 Cantidad = detalleVenta.Cantidad,
                                 PrecioUnitario = detalleVenta.PrecioUnitario,
                                 Valor = detalleVenta.Valor,
                                 Estilo = stockProducto.Estilo,
                                 Descripcion = stockProducto.Descripcion
                             };

            // Asignar los datos al DataGrid
            dgventas.ItemsSource = dataToShow;
        }



        private void CalcularTotales()
        {
            int totalCantidad = 0;
            int totalValor = 0;

            // Verificar que el DataGrid tiene elementos
            if (dgventas.Items.Count > 0)
            {
                // Recorrer las filas del DataGrid y sumar los valores de Cantidad y Valor
                foreach (var item in dgventas.Items)
                {
                    if (item is DetallesVentaConStock detalleVenta)
                    {
                        totalCantidad += detalleVenta.Cantidad;
                        totalValor += (int)detalleVenta.Valor; // Suponiendo que Valor es de tipo decimal, se convierte a int para la suma
                    }
                }
            }

            // Mostrar los resultados
            float totalIvaF = (float)totalValor / 11.0f;
            int totalIVA = (int)Math.Ceiling(totalIvaF);
            txttotalproductos.Text = totalCantidad.ToString();
            txttotalmonto.Text = totalValor.ToString();
            txttotaliva.Text = totalIVA.ToString();
            Venta venta = new Venta();
            venta.TotalIva = totalIVA;
            venta.MontoTotal = totalValor;
            venta.IdVenta = int.Parse(txtidventa.Text);
            ventaDao.ActualizarMontoTotalYTotalIva(venta);
        }





        private void btnañadirventa_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtidcliente.Text))
            {
                // Bloque para agregar un nuevo cliente
                if (txtnombre.Text == "")
                {
                    MessageBox.Show("Ingrese el nombre del cliente", "CraftBeer Manager");
                    txtnombre.Focus();
                    return;
                }
                if (txtruc.Text == "")
                {
                    MessageBox.Show("Ingrese el RUC del cliente", "CraftBeer Manager");
                    txtruc.Focus();
                    return;
                }
                if (txtdireccion.Text == "")
                {
                    MessageBox.Show("Ingrese la direccion del cliente", "CraftBeer Manager");
                    txtdireccion.Focus();
                    return;
                }
                if (txttelefono.Text == "")
                {
                    MessageBox.Show("Ingrese el telefono del cliente", "CraftBeer Manager");
                    txttelefono.Focus();
                    return;
                }

                // Crear el cliente y guardarlo en la base de datos
                Clientes cli = new Clientes();
                cli.RUC = txtruc.Text;
                cli.Nombre = txtnombre.Text;
                cli.Direccion = txtdireccion.Text;
                cli.Telefono = txttelefono.Text;
                if (clientesDao.Insert(cli, out int idCliente))
                {
                    txtidcliente.Text = idCliente.ToString();
                    MessageBox.Show("Cliente ingresado con exito", "CraftBeer Manager");
                }
                else
                {
                    MessageBox.Show("Error al añadir el cliente", "CraftBeer Manager");
                    return;
                }
            }

            // Ahora, agregar una nueva venta para el cliente existente o nuevo
            Venta venta = new Venta();

            DateTime fechaActual = DateTime.Today;
            DateTime soloFecha = fechaActual.Date;
            txtfecha.Text = soloFecha.ToString("dd/MM/yyyy");
            venta.Fecha = DateTime.Parse(txtfecha.Text);
            venta.IdCliente = int.Parse(txtidcliente.Text);
            if (ventaDao.Insert(venta, out int idVenta))
            {
                txtidventa.Text = idVenta.ToString();
                //MessageBox.Show("Agregado correctamente", "CraftBeer Manager");
            }
            else
            {
                MessageBox.Show("Error al añadir la venta", "CraftBeer Manager");
            }
        }


        private void btnañadirproducto_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtidcliente.Text) || string.IsNullOrEmpty(txtfecha.Text) || string.IsNullOrEmpty(txtidcliente.Text)
                || string.IsNullOrEmpty(txtruc.Text) || string.IsNullOrEmpty(txtnombre.Text) || string.IsNullOrEmpty(txtdireccion.Text)
                || string.IsNullOrEmpty(txttelefono.Text) || string.IsNullOrEmpty(txtidproducto.Text) || string.IsNullOrEmpty(cbproductos.Text)
                || string.IsNullOrEmpty(txtdescripcion.Text) || string.IsNullOrEmpty(txtcantidad.Text) || string.IsNullOrEmpty(cbtipoventa.Text))
            {
                MessageBox.Show("Por Favor Seleccione todos los campos", "CraftBeer Manager");
                return;
            }
            string descrip = txtdescripcion.Text;
            DetallesVenta detalles = new DetallesVenta();
            detalles.IdVenta = int.Parse(txtidventa.Text);
            detalles.IdEstilo = int.Parse(txtidproducto.Text);
            int cantdisp = int.Parse(txtcantidaddisp.Text);
            int cantacarg = int.Parse(txtcantidad.Text);
            if (cantdisp >= cantacarg)
            {
                detalles.Cantidad = int.Parse(txtcantidad.Text);
            }
            else
            {
                MessageBox.Show("No hay Stock Suficiente", "CraftBeer Manager");
            }
            string itemSeleccionado = cbtipoventa.SelectedValue.ToString();
            if (itemSeleccionado == "Minorista")
            {
                if (descrip=="Botellas 500ml")
                {
                    int preciominiorista = 20000;
                    detalles.PrecioUnitario = preciominiorista;
                    int valorf = preciominiorista * int.Parse(txtcantidad.Text);
                    detalles.Valor = valorf;
                }
                else if(descrip =="Barriles 20L")
                {
                    int preciominorista = 450000;
                    detalles.PrecioUnitario = preciominorista;
                    int valorf = preciominorista * int.Parse(txtcantidad.Text);
                    detalles.Valor=valorf;
                }
                
            }
            else if(itemSeleccionado == "Mayorista")
            {
                if (descrip == "Botellas 500ml")
                {
                    int preciomayorista = 16000;
                    detalles.PrecioUnitario = preciomayorista;
                    int valorf = preciomayorista * int.Parse(txtcantidad.Text);
                    detalles.Valor = valorf;
                }
                else if (descrip == "Barriles 20L")
                {
                    int preciomayorista = 420000;
                    detalles.PrecioUnitario = preciomayorista;
                    int valorf = preciomayorista * int.Parse(txtcantidad.Text);
                    detalles.Valor = valorf;
                }
            }
            
            

            if (detallesDao.InsertarDetalleVenta(detalles))
            {
                //MessageBox.Show("Producto Ingresado con exito", "CraftBeer Manager");
                CargarDatos();
                CargarDataGrid();
                

                int idStockProducto = int.Parse(txtidproducto.Text);
                int cantidadRestar = int.Parse(txtcantidad.Text);
                if (stockProductoDao.RestarProductos(idStockProducto, cantidadRestar))
                {
                    //MessageBox.Show("Se resto la cantidad a stock producto", "CraftBeer Manager");
                    CalcularTotales();
                    txtiddetallesventa.Clear();
                    txtidproducto.Clear();
                    cbproductos.SelectedItem = null;
                    txtdescripcion.Clear();
                    txtcantidad.Clear();
                    txtcantidaddisp.Clear();
                    cbtipoventa.SelectedItem = null;



                }
                else
                {
                    MessageBox.Show("Error al restar", "CraftBeer Manager");
                }

            }




        }

        private void btnimprimirfactura_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los datos de las tablas Ventas, DetallesVenta y StockProducto desde la base de datos
            int idVenta = int.Parse(txtidventa.Text);

            // Obtener los datos de la tabla Ventas
            List<Venta> listaVentas = ventaDao.GetVentaById(idVenta);

            // Obtener los datos de la tabla DetallesVenta
            List<DetallesVenta> detalles = detallesDao.GetDetallesVentaPorIdVenta(idVenta);

            // Obtener los datos de la tabla StockProducto
            List<int> idEstilos = detalles.Select(d => d.IdEstilo).ToList();
            List<StockProducto> productos = stockProductoDao.GetStockProductosPorIds(idEstilos);

            // Crear el archivo de texto para la factura
            string contenidoFactura = "CraftBeer Manager - Factura" + Environment.NewLine;
            contenidoFactura += "------------------------------" + Environment.NewLine;
            contenidoFactura += "Fecha: " + txtfecha.Text + Environment.NewLine;
            contenidoFactura += "RUC: " + txtruc.Text + Environment.NewLine;
            contenidoFactura += "Nombre: " + txtnombre.Text + Environment.NewLine;
            contenidoFactura += "Dirección: " + txtdireccion.Text + Environment.NewLine;
            contenidoFactura += "Teléfono: " + txttelefono.Text + Environment.NewLine;
            contenidoFactura += "Factura Número: " + txtidventa.Text + Environment.NewLine;
            contenidoFactura += "------------------------------" + Environment.NewLine;
            contenidoFactura += "Detalles de la Venta:" + Environment.NewLine;
            contenidoFactura += "IdEstilo | Cantidad | Precio Unitario | Valor | Estilo | Detalles" + Environment.NewLine;

            bool isTotalPrinted = false; // Variable para controlar si el bloque "Total IVA" y "Monto Total" ya se imprimió o no

            foreach (Venta vent in listaVentas)
            {
                contenidoFactura += "------------------------------" + Environment.NewLine;

                // Obtener los detalles para esta venta
                var detallesVenta = detalles.Where(d => d.IdVenta == vent.IdVenta).ToList();

                foreach (DetallesVenta detalle in detallesVenta)
                {
                    StockProducto producto = productos.FirstOrDefault(p => p.IdEstilo == detalle.IdEstilo);

                    if (producto != null)
                    {
                        contenidoFactura += $"{detalle.IdEstilo} | {detalle.Cantidad} | {detalle.PrecioUnitario} | {detalle.Valor} | {producto.Estilo} | {producto.Descripcion}" + Environment.NewLine;
                    }
                    else
                    {
                        // Si no se encontró el producto, mostrar un mensaje o un valor predeterminado
                        contenidoFactura += $"{detalle.IdEstilo} | {detalle.Cantidad} | {detalle.PrecioUnitario} | {detalle.Valor} | Producto no encontrado | -" + Environment.NewLine;
                    }
                }

                // Si no se ha impreso el bloque "Total IVA" y "Monto Total", imprimirlo solo una vez
                /*
                if (!isTotalPrinted)
                {
                    contenidoFactura += "Total IVA: " + vent.TotalIva + Environment.NewLine;
                    contenidoFactura += "Monto Total: " + vent.MontoTotal + Environment.NewLine;
                    isTotalPrinted = true;
                }*/
            }

            // Imprimir los totales generales al final
            contenidoFactura += "------------------------------" + Environment.NewLine;
            contenidoFactura += "Total IVA Total: " + listaVentas.Sum(v => v.TotalIva) + Environment.NewLine;
            contenidoFactura += "Monto Total Total: " + listaVentas.Sum(v => v.MontoTotal) + Environment.NewLine;

            // Guardar el contenido en un archivo de texto
            string nombreArchivo = "factura_" + idVenta.ToString() + ".txt";
            // Reemplaza Path.Combine con System.IO.Path.Combine
            string rutaArchivo = System.IO.Path.Combine("C:\\Users\\Doomed666\\source\\repos\\CraftBeer\\CraftBeer\\Facturas\\", nombreArchivo);
            File.WriteAllText(rutaArchivo, contenidoFactura);

            // Mostrar un mensaje de éxito
            MessageBox.Show("Factura generada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            string rutaArchivoCompleta = System.IO.Path.Combine("C:\\Users\\Doomed666\\source\\repos\\CraftBeer\\CraftBeer\\Facturas\\", nombreArchivo);
            System.Diagnostics.Process.Start(rutaArchivoCompleta);

            LimpiarTodo();

        }

        private void LimpiarTodo()
        {
            txtidcliente.Clear();
            txtiddetallesventa.Clear();
            txtidproducto.Clear();
            txtidventa.Clear();
            txtnombre.Clear();
            txtruc.Clear();
            txtdescripcion.Clear();
            txtcantidad.Clear();
            txtdireccion.Clear();
            txttelefono.Clear();
            txttotaliva.Clear();
            txttotalmonto.Clear();
            txttotalproductos.Clear();
            txtcantidaddisp.Clear();
            cbproductos.SelectedItem = null;
            cbtipoventa.SelectedItem = null;
            txtidcliente.Focus();
            dgventas.ItemsSource = null;
        }


        private void txtidcliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int idCliente;
                if (int.TryParse(txtidcliente.Text, out idCliente))
                {
                    Clientes cliente = clientesDao.GetClienteById(idCliente);
                    if (cliente != null)
                    {
                        txtruc.Text = cliente.RUC;
                        txtnombre.Text = cliente.Nombre;
                        txtdireccion.Text = cliente.Direccion;
                        txttelefono.Text = cliente.Telefono;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un cliente con el ID especificado", "CraftBeer Manager");
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese un ID de cliente válido", "CraftBeer Manager");
                }
            }
        }

        private void txtruc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string ruc = txtruc.Text.Trim();
                if (!string.IsNullOrEmpty(ruc))
                {
                    Clientes cliente = clientesDao.GetClienteByRuc(ruc);
                    if (cliente != null)
                    {
                        txtidcliente.Text = cliente.IdCliente.ToString();
                        txtnombre.Text = cliente.Nombre;
                        txtdireccion.Text = cliente.Direccion;
                        txttelefono.Text = cliente.Telefono;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un cliente con el RUC especificado, complete los campos para añadir un nuevo cliente", "CraftBeer Manager");
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese un RUC válido", "CraftBeer Manager");
                }
            }
        }

        private void cbproductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbproductos.SelectedItem != null)
            {
                string estiloSeleccionado = cbproductos.SelectedItem.ToString();
                StockProducto stockSeleccionado = listaStock.FirstOrDefault(l => l.Estilo == estiloSeleccionado);
                if (stockSeleccionado != null)
                {
                    txtdescripcion.Text = stockSeleccionado.Descripcion.ToString();
                    txtcantidaddisp.Text = stockSeleccionado.Cantidad.ToString();
                    txtidproducto.Text = stockSeleccionado.IdEstilo.ToString();
                }
            }
        }

        private void btnEditarVentas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminarVentas_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
    /// Lógica de interacción para pageDashboard.xaml
    /// </summary>
    public partial class pageDashboard : Page
    {
        VentaDao ventaDao = new VentaDao();
        ClientesDao clienteDao = new ClientesDao();
        LotesDao loteDao = new LotesDao();
        StockProductoDao stockprodDao = new StockProductoDao();
        LoginDao loginDao = new LoginDao();
        public pageDashboard()
        {
            InitializeComponent();

            // OBTENER ULTIMAS VENTAS
            List<Venta> ultimasVentas = ventaDao.GetUltimasTresVentas();

            if (ultimasVentas.Count >= 3)
            {
                Venta primeraVenta = ultimasVentas[0];
                Venta segundaVenta = ultimasVentas[1];
                Venta terceraVenta = ultimasVentas[2];

                int id1 = primeraVenta.IdCliente;
                int id2 = segundaVenta.IdCliente;
                int id3 = terceraVenta.IdCliente;

                //Clientes clienteDao = new Clientes(); // Suponiendo que tienes un objeto clienteDao para acceder a la entidad Clientes

                string nombre1 = clienteDao.GetClienteById(id1).Nombre;
                string nombre2 = clienteDao.GetClienteById(id2).Nombre;
                string nombre3 = clienteDao.GetClienteById(id3).Nombre;

                int monto1 = primeraVenta.MontoTotal ?? 0;
                int monto2 = segundaVenta.MontoTotal ?? 0;
                int monto3 = terceraVenta.MontoTotal ?? 0;


                string fecha1 = primeraVenta.Fecha.ToString();
                string fecha2 = segundaVenta.Fecha.ToString();
                string fecha3 = terceraVenta.Fecha.ToString();

                lblventa1.Content = "Nombre Cliente: " + nombre1 + " en la Fecha " + fecha1 + " por el monto de " + monto1;
                lblventa2.Content = "Nombre Cliente: " + nombre2 + " en la Fecha " + fecha2 + " por el monto de " + monto2;
                lblventa3.Content = "Nombre Cliente: " + nombre3 + " en la Fecha " + fecha3 + " por el monto de " + monto3;
            }


            // OBTENER ULTIMOS LOTES
            List<Lotes> ultimosTres = loteDao.GetUltimosTresLotes();
            if (ultimosTres.Count >= 3)
            {
                Lotes primerRegistro = ultimosTres[0];
                Lotes segundoRegistro = ultimosTres[1];
                Lotes tercerRegistro = ultimosTres[2];

                string estilo1 = primerRegistro.Estilo;
                string maestro1 = primerRegistro.Maestro;
                string fecha1 = primerRegistro.FechaInicio.ToString("dd/MM/yyyy");

                string estilo2 = segundoRegistro.Estilo;
                string maestro2 = segundoRegistro.Maestro;
                string fecha2 = segundoRegistro.FechaInicio.ToString("dd/MM/yyyy");

                string estilo3 = tercerRegistro.Estilo;
                string maestro3 = tercerRegistro.Maestro;
                string fecha3 = tercerRegistro.FechaInicio.ToString("dd/MM/yyyy");

                lbllote1.Content = estilo1 + "  Elaborado por " + maestro1 + "  el  " + fecha1;
                lbllote2.Content = estilo2 + "  Elaborado por " + maestro2 + "  el  " + fecha2;
                lbllote3.Content = estilo3 + "  Elaborado por " + maestro3 + "  el  " + fecha3;
            }


            // OBTENER ULTIMOS PRODUCTOS
            List<StockProducto> ultimosStockProductos = stockprodDao.GetUltimosTresStockProductos();
            if (ultimosStockProductos.Count >= 3)
            {
                StockProducto primerStock = ultimosStockProductos[0];
                StockProducto segundoStock = ultimosStockProductos[1];
                StockProducto tercerStock = ultimosStockProductos[2];

                string estiloProd1 = primerStock.Estilo;
                string estiloProd2 = segundoStock.Estilo;
                string estiloProd3 = tercerStock.Estilo;

                lblprod1.Content = estiloProd1;
                lblprod2.Content = estiloProd2;
                lblprod3.Content = estiloProd3;
            }



        }

        public void getUser(string user)
        {
            int iduser = loginDao.GetIdUsuarioByUsuario(user);
            Login login = loginDao.GetUsuarioById(iduser);
            string nombre = login.Nombre;
            string apellido = login.Apellido;

            lblbienvenida.Content = "Bienvenido/a  " +nombre+" "+apellido+" a CraftBeer Manager";

        }
    }
}

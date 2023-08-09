using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class Factura
    {
        public DateTime Fecha { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int FacturaNumero { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public string Detalles { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }
        public int Valor { get; set; }
    }
}
